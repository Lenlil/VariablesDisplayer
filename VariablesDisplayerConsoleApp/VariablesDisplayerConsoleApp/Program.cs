using System;
using System.Collections.Generic;

namespace VariablesDisplayerConsoleApp
{
    class Program
    {
        static List<Variable> existingVariables = new List<Variable>();
        static List<string> outputStrings = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please write below:");

            while (true)
            {             
                var input = Console.ReadLine();

                var output = GetOutput(input);

                if (output != null)
                {
                    Console.WriteLine(output);
                    foreach (var text in outputStrings)
                    {
                        Console.WriteLine(text);
                    }
                    outputStrings.Clear();
                }                             
            }        
        }

        static string GetOutput(string input)
        {            
            char[] equalSeperator = new char[] { '=' };
            char[] additionSeperator = new char[] { '+' };
            string result = input.Replace(" ", "");
            string[] inputArray = result.Split(equalSeperator);       
            var firstVariable = inputArray[0];
            var restOfInput = inputArray[1];
            string[] additionsArray = restOfInput.Split(additionSeperator);

            int intValue = 0;
            string[] stringValues = new string[0];     

            for (int i = 0; i < additionsArray.Length; i++)
            {
                int value;
                if (int.TryParse(additionsArray[i], out value))
                {
                    intValue += value;
                }
                else
                {
                    Array.Resize(ref stringValues, stringValues.Length + 1);
                    stringValues[stringValues.Length-1] = additionsArray[i];                  
                }
            }               
          
            if (!VariableAlreadyExist(firstVariable))
            {
                AddVariableToExistingVariable(firstVariable, intValue, stringValues);              
            }
            else
            {
                UpdateFirstVariable(firstVariable, intValue, stringValues);
            }

            var output = OutputToBeDisplayedForVariable(firstVariable);

            return output;
        }

        static string OutputToBeDisplayedForVariable(string variableName)
        {
            Variable variable = existingVariables.Find(e => e.Name == variableName);          

            if (variable.HasVariableValues)
            {
                return null;
            }
            else
            {
                var outputString = $"{variable.Name} = {variable.Value}";
                return outputString;
            }
        }

        static void UpdateFirstVariable(string variableName, int intValue, string[] stringValues)
        {
            var variableToUpdate = existingVariables.Find(e => e.Name == variableName);

            variableToUpdate.Value += intValue;

            if (stringValues.Length != 0)
            {
                List<string> stringValuesList = new List<string>(stringValues);
                variableToUpdate.VariableValues.AddRange(stringValuesList);
            }                 
        }

        static bool VariableAlreadyExist(string variable)
        {
            if (existingVariables.Find(item => item.Name == variable) != null)
            {
                return true;
            }
            else
            {
                return false;
            }                 
        }

        static void AddVariableToExistingVariable(string variableName, int intValue, string[] stringValues)
        {      
            if (stringValues.Length != 0)
            {
                List<string> stringValuesList = new List<string>(stringValues);

                for (int i = stringValuesList.Count - 1; i >= 0; i--)
                {
                    if (existingVariables.Exists(x => x.Name == stringValuesList[i]))
                    {
                        intValue += existingVariables.Find(x => x.Name == stringValuesList[i]).Value;
                        stringValuesList.Remove(stringValuesList[i]);
                    }
                }

                if (stringValuesList.Count == 0)
                {
                    existingVariables.Add(new Variable()
                    {
                        Name = variableName,
                        Value = intValue,                       
                        HasVariableValues = false,
                    });
                }
                else
                {
                    existingVariables.Add(new Variable()
                    {
                        Name = variableName,
                        Value = intValue,
                        VariableValues = stringValuesList,
                        HasVariableValues = true,
                    });
                }              
            }
            else
            {
                existingVariables.Add(new Variable()
                {
                    Name = variableName,
                    Value = intValue,
                    HasVariableValues = false,
                });

                UpdateOtherVariablesThatHasVariableInVariableValues(variableName, intValue);
            }         
        }     

        static void UpdateOtherVariablesThatHasVariableInVariableValues(string variableName, int intValue)
        {
            var variablesToCheck = existingVariables.FindAll(x => x.HasVariableValues);

            foreach (var variable in variablesToCheck)
            {
                variable.VariableValues.Remove(variableName);
                variable.Value += intValue;
                if (variable.VariableValues.Count == 0)
                {
                    variable.HasVariableValues = false;
                    outputStrings.Add($"{variable.Name} = {variable.Value}");
                }
            }       
        }
    }
}
