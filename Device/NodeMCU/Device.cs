using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Firestore;

namespace NodeMCU
{
    [FirestoreData]
    public class Device
    {
        [FirestoreProperty]
        public double pmax { get; set; }
        [FirestoreProperty]
        public double pmin { get; set; }
        [FirestoreProperty]
        public double volume { get; set; }
    }
}