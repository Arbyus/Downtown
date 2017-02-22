using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Linq;

public class Beater : MonoBehaviour {
    static double[] energySubbands = new double[32];
    static double[][] subbandHistory = new double[32][];
    static Complex[] inputs = new Complex[1024];
    static double[] Buffer = new double[1024];
    static int point = 0;
    static int filePointer = 0;
    bool loaded = false;
    public AudioSource audioToBeatTo;
    public ObjectsController objC;
    public float TESTV = 6.5f;

    float[] songDataFlt = new float[2048];
    double[] songDataStr = new double[2048];

    public int BitReverse(int n, int bits)
    {
        int reversedN = n;
        int count = bits - 1;

        n >>= 1;
        while (n > 0)
        {
            reversedN = (reversedN << 1) | (n & 1);
            count--;
            n >>= 1;
        }

        return ((reversedN << count) & ((1 << bits) - 1));
    }

    public void FFT(Complex[] buffer)
    {

        int bits = (int)Math.Log(buffer.Length, 2);
        for (int j = 1; j < buffer.Length / 2; j++)
        {

            int swapPos = BitReverse(j, bits);
            var temp = buffer[j];
            buffer[j] = buffer[swapPos];
            buffer[swapPos] = temp;
        }

        for (int N = 2; N <= buffer.Length; N <<= 1)
        {
            for (int i = 0; i < buffer.Length; i += N)
            {
                for (int k = 0; k < N / 2; k++)
                {

                    int evenIndex = i + k;
                    int oddIndex = i + k + (N / 2);
                    var even = buffer[evenIndex];
                    var odd = buffer[oddIndex];

                    double term = -2 * Math.PI * k / (double)N;
                    Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

                    buffer[evenIndex] = even + exp;
                    buffer[oddIndex] = even - exp;

                }
            }
        }
    }

    public bool AddE()
    {
        try
        {
            if (filePointer + 2048 > audioToBeatTo.clip.samples)
            {
                return false;
            }
            for (int i = 0; i < 2048; i += 2)
            {
                inputs[point] = (songDataStr[i]) + (i * (songDataStr[i + 1]));
                ++point;
                if (point == 1024)
                {
                    break;
                }
            }
            point = 0;
        }
        catch (IndexOutOfRangeException)
        {
           
        }

        FFT(inputs);

        for (int i = 0; i < 1024; ++i)
        {
            Buffer[i] = Math.Pow(Complex.Abs(inputs[i]), 2);
        }

        for (int i = 0; i < 32; ++i)
        {
            double result = 0;
            for (int j = i * 32; j < (i + 1) * 32; ++j)
            {
                result += Buffer[j];
            }

            energySubbands[i] = (32.0 / 1024.0) * result;
        }

        double[] results = new double[32];

        for (int i = 0; i < 32; ++i)
        {
            double av = 0;
            for (int j = 0; j < 42; ++j)
            {
                av += subbandHistory[i][j];
            }
            results[i] = (1.0 / 43.0) * av;
        }

        foreach (double[] arr in subbandHistory)
        {
            Array.Copy(arr, 0, arr, 1, arr.Length - 1);
            arr[0] = 0;
        }

        for (int i = 0; i < 32; ++i)
        {
            subbandHistory[i][0] = energySubbands[i];
            double hs = TESTV * results[i];
            if (energySubbands[i] > hs)
            {
                // Console.WriteLine("We got one");
                return true;
            }
            else
            {
                //Debug.Log(hs - energySubbands[i]);
            }
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 32; ++i)
        {
            subbandHistory[i] = new double[43];
        }
        loaded = true;
        audioToBeatTo.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (loaded && IsBeat())
        {
            objC.Beat();
        }
    }

    public bool IsBeat()
    {
        audioToBeatTo.clip.GetData(songDataFlt, audioToBeatTo.timeSamples);
        for (int i = 0; i < songDataFlt.Length; ++i)
        {
            songDataStr[i] = songDataFlt[i];
        }
        int sampleDiff = audioToBeatTo.timeSamples - filePointer;
        filePointer = audioToBeatTo.timeSamples;

        if (sampleDiff > 2048)
        {
            sampleDiff = 2048;
        }
        else if(sampleDiff < 0)
        {
            sampleDiff = 0;
        }

        for(int i = 0; i < sampleDiff; ++i)
        {
            Array.Copy(songDataStr, sampleDiff, songDataStr, 0, songDataStr.Length-sampleDiff);
        }

        return AddE();
    }
}