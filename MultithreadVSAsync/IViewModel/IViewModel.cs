using System;
using System.Collections.Generic;
using System.Text;

namespace MultithreadVSAsync.IViewModel
{
    internal interface IViewModel
    {
        public abstract void Start();
        public static abstract void Choose();
        public static abstract void ApplyChoice(string choice);
    }
}
