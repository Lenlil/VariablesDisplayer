using System;
using System.Collections.Generic;
using System.Text;

namespace VariablesDisplayerConsoleApp
{
    class Variable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public List<string> VariableValues { get; set; }
        public bool HasVariableValues { get; set; } 
    }
}
