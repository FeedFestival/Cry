using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    public static class MindMap
    {
        public static bool isPlaying;

        private static Neuron _guardNeuron;
        public static Neuron GuardNeuron
        {
            get
            {
                if (_guardNeuron == null)
                    _guardNeuron = InitGuard();
                return _guardNeuron;
            }
        }

        public static Neuron GetGuardNeuron(Guard guard)
        {
            return InitGuard(guard);
        }

        private static NeuronResult nothing()
        {
            return NeuronResult.WaitFor;
        }

        public static Neuron InitGuard(Guard guard = null)
        {
            var guardNeuron = new Neuron
            {
                Id = null,
                NeuronType = NeuronType.Root,
                DebugText = "Root",
                Children = new List<Neuron>
                {
                    new Neuron
                    {
                        Id = 15,
                        NeuronType = NeuronType.If,
                        DebugText = "Fighting",
                        Method = () => guard == null ? nothing() : guard.CheckStance(Stance.Fighting),
                        Children = new List<Neuron>
                        {
                            new Neuron
                            {
                                Id = 18,
                                NeuronType = NeuronType.Action,
                                DebugText = "Job",
                                Method = () => guard == null ? nothing() : guard.DoJob()
                            }
                        }
                    },
                    new Neuron
                    {
                        Id = 16,
                        NeuronType = NeuronType.If,
                        DebugText = "Pasive",
                        Method = () => guard == null ? nothing() : guard.CheckStance(Stance.Pasive),
                    //-------------------------------------------------------------------   If Pasive                  ---------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        Children = new List<Neuron>
                        {
                            new Neuron
                            {
                                Id = 17,
                                NeuronType = NeuronType.Selector,
                                Children = new List<Neuron>
                                {
                                    new Neuron
                                    {
                                        Id = 1,
                                        NeuronType = NeuronType.If,
                                        DebugText = "Alerted",
                                        Method = () => guard == null ? nothing() : guard.CheckMainState(MainState.Alerted),
                                    //-------------------------------------------------------------------   If Alerted                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                        Children = new List<Neuron>
                                        {
                                            new Neuron
                                            {
                                                Id = 6,
                                                NeuronType = NeuronType.Sequence,
                                                Children = new List<Neuron>
                                                {
                                                    new Neuron
                                                    {
                                                        Id = 9,
                                                        NeuronType = NeuronType.Action,
                                                        DebugText = "Check Alert",
                                                        Method = () => guard == null ? nothing() : guard.CheckAlert()
                                                    },
                                                    new Neuron
                                                    {
                                                        Id = 10,
                                                        NeuronType = NeuronType.IfElse,
                                                        DebugText = "Seeing player",
                                                        Method = () => guard == null ? nothing() : guard.CheckAlertType(Alert.Seeing),
                                                        Children = new List<Neuron>
                                                        {
                                                            // true
                                                            new Neuron
                                                            {
                                                                Id = 11,
                                                                NeuronType = NeuronType.Sequence,
                                                                Children = new List<Neuron>
                                                                {
                                                                    new Neuron
                                                                    {
                                                                        Id = 13,
                                                                        NeuronType = NeuronType.Action,
                                                                        DebugText = "Set Alert Level Talkative",
                                                                        Method = () => guard == null ? nothing() : guard.SetAlertLevel(AlertLevel.Talkative),
                                                                    },
                                                                    new Neuron
                                                                    {
                                                                        Id = 14,
                                                                        NeuronType = NeuronType.Action,
                                                                        DebugText = "Reduce Distance",
                                                                        Method = () => guard == null ? nothing() : guard.ReduceDistance(),
                                                                    }
                                                                }
                                                            },
                                                            // false
                                                            new Neuron
                                                            {
                                                                Id = 12,
                                                                NeuronType = NeuronType.Action,
                                                                DebugText = "Set Alert Level None",
                                                                Method = () => guard == null ? nothing() : guard.WaitSetAlertLevel(AlertLevel.None)
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    //-------------------------------------------------------------------   END If Alerted                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                    new Neuron
                                    {
                                        Id = 2,
                                        NeuronType = NeuronType.If,
                                        DebugText = "Investigative",
                                        Method = () => guard == null ? nothing() : guard.CheckMainState(MainState.Investigative),
                                    //-------------------------------------------------------------------   If Investigative                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                        Children = new List<Neuron>
                                        {
                                            new Neuron
                                            {
                                                Id = 7,
                                                NeuronType = NeuronType.Sequence,
                                                Children = new List<Neuron>
                                                {
                                                    new Neuron
                                                    {
                                                        Id = 19,
                                                        NeuronType = NeuronType.Action,
                                                        DebugText = "Check Suroundings",
                                                        Method = () => guard == null ? nothing() : guard.CheckSuroundings(),
                                                    },
                                                    new Neuron
                                                    {
                                                        Id = 20,
                                                        NeuronType = NeuronType.Action,
                                                        DebugText = "Investigate Sound Location",
                                                        Method = () => guard == null ? nothing() : guard.InvestigateSoundLocation(),
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    //-------------------------------------------------------------------   END If Investigative                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                    new Neuron
                                    {
                                        Id = 3,
                                        NeuronType = NeuronType.If,
                                        DebugText = "Aggressive",
                                        Method = () => guard == null ? nothing() : guard.CheckMainState(MainState.Aggressive),
                                        //-------------------------------------------------------------------   If Aggressive                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                        Children = new List<Neuron>
                                        {
                                            new Neuron
                                            {
                                                Id = 8,
                                                NeuronType = NeuronType.Sequence,
                                                Children = new List<Neuron>
                                                {
                                                    new Neuron
                                                    {
                                                        Id = 21,
                                                        NeuronType = NeuronType.Action,
                                                        DebugText = "Set Alert Level Aggressive",
                                                        Method = () => guard == null ? nothing() : guard.SetAlertLevel(AlertLevel.Aggressive)
                                                    },
                                                    new Neuron
                                                    {
                                                        Id = 22,
                                                        NeuronType = NeuronType.IfElse,
                                                        DebugText = "Seeing player",
                                                        Method = () => guard == null ? nothing() : guard.CheckAlertType(Alert.Seeing),
                                                        Children = new List<Neuron>
                                                        {
                                                            // true
                                                            new Neuron
                                                            {
                                                                Id = 23,
                                                                NeuronType = NeuronType.Action,
                                                                DebugText = "Chase Player",
                                                                Method = () => guard == null ? nothing() : guard.ChasePlayer()
                                                            },
                                                            // false
                                                            new Neuron
                                                            {
                                                                Id = 24,
                                                                NeuronType = NeuronType.Action,
                                                                DebugText = "Investigate last known location",
                                                                Method = () => guard == null ? nothing() : guard.DoInvestigateLastKnownLocation()
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    //-------------------------------------------------------------------   END If Aggressive                  ---------------------------------------------------------------------------
                                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                    new Neuron
                                    {
                                        Id = 4,
                                        NeuronType = NeuronType.If,
                                        DebugText = "Calm",
                                        Method = () => guard == null ? nothing() : guard.CheckMainState(MainState.Calm),
                                        Children = new List<Neuron>
                                        {
                                            new Neuron
                                            {
                                                Id = 5,
                                                NeuronType = NeuronType.Action,
                                                DebugText = "Job",
                                                Method = () => guard == null ? nothing() : guard.DoJob()
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //-------------------------------------------------------------------   End if Pasive              ---------------------------------------------------------------------------
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                }
            };

            SetParents(guardNeuron);

            if (guard != null)
            {
                guard.DisplayErrors(errors);
            }

            return guardNeuron;
        }

        private static List<string> errors;
        private static List<int> ids;

        private static void SetParents(Neuron parentNeuron)
        {
            if (ids == null)
            {
                ids = new List<int>();
                errors = new List<string>();
            }

            CheckForErrors(parentNeuron);

            // after we check for id we add it
            if (parentNeuron.Id != null)
                ids.Add(parentNeuron.Id.Value);

            foreach (var neuron in parentNeuron.Children)
            {
                if (neuron.Children != null)
                    SetParents(neuron);
                neuron.Parent = parentNeuron;
            }
        }

        private static void CheckForErrors(Neuron neuron)
        {
            if (neuron.Id != null && ids.Contains(neuron.Id.Value))
                errors.Add("Id duplication : " + neuron.Id);

            if (neuron.NeuronType == NeuronType.If)
            {
                if (neuron.Children == null || neuron.Children.Count == 0)
                {
                    errors.Add("One If (Neuron) has no Action");
                }
            }
        }
    }
}
