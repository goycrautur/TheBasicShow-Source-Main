using System;

[Serializable]
    public class Requirements
    {
        public NeedTypes itNeedSmth;
        public enum NeedTypes {String,Bool,Int};
        public string ReqStringPPrefs,PPrefsStringVal,ReqBoolPPrefsName,ReqIntPPrefsName;
        public int PPrefsIntVal;
    }
