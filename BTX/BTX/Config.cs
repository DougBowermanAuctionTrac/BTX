﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BTX
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Config();
                }
                return _instance;
            }
        }
        public string dbPath { get; set; }
    }
}
