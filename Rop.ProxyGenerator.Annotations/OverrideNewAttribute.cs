﻿using System;

namespace Rop.ProxyGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Property)]
    public class OverrideNewAttribute : Attribute { }
}