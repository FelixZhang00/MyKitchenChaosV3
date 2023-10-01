using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<ProgressEventArgs> OnProgressChanged;
    public class ProgressEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
