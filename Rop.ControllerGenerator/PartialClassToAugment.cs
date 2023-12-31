﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rop.GeneratorShared;

namespace Rop.ControllerGenerator
{
    public class PartialClassToAugment:BasePartialClassToAugment
    {
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment) : base(classToAugment)
        {
        }
        public IEnumerable<string> GetHeader(IEnumerable<string> additionalusings)
        {
            return GetHeader0().Concat(GetHeader1()).Concat(GetNamespace0()).Concat(GetClass0());
            // Local functions
            IEnumerable<string> GetHeader1()
            {
                foreach (var additionalusing in additionalusings)
                {
                    yield return $"using {additionalusing};";
                }
            }
        }

      
    }
}
