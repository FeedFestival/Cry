using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Utils;
using BAD;

public class Intel : MonoBehaviour
{
    public bool DebugThis;

    void Awake()
    {
        DebugThis = true;
    }

    private UnitInteligence _unitInteligence;
    private Guard _guard;
    [HideInInspector]
    public Guard Guard
    {
        get
        {
            if (_guard == null)
                _guard = GetComponent<Guard>();
            return _guard;
        }
    }

    public void Initialize(UnitInteligence unitInteligence)
    {
        _unitInteligence = unitInteligence;

        gameObject.AddComponent<Guard>();
        Guard.Initialize();


        // Start
        Run(Guard.GuardNeuron);
    }

    private Neuron _currentNeuron;

    public void Run(Neuron neuron)
    {
        _currentNeuron = neuron;

        if (neuron.NeuronState == NeuronState.Complete)
            CompleteNeuronBranch();

        neuron.NeuronState = NeuronState.Running;

        switch (neuron.NeuronType)
        {
            case NeuronType.Root:
                NextChild();
                break;

            case NeuronType.Selector:
                NextChild();
                break;

            case NeuronType.Sequence:
                NextChild();
                break;

            case NeuronType.If:
                neuron.NeuronResult = neuron.Method();

                if (neuron.NeuronResult == NeuronResult.Success)
                    Run(neuron.Children.First());
                else
                    CompleteNeuronBranch();
                break;

            case NeuronType.IfElse:
                neuron.NeuronResult = neuron.Method();

                if (neuron.NeuronResult == NeuronResult.Success)
                    Run(neuron.Children.First());
                else
                    Run(neuron.Children.Last());
                break;

            case NeuronType.Action:
                neuron.NeuronResult = neuron.Method();

                if (neuron.NeuronResult == NeuronResult.Continue)
                    CompleteNeuronBranch();
                break;
        }
    }

    private void NextChild()
    {
        int remainingNeurons = _currentNeuron.Children.Count(a => a.NeuronState == NeuronState.NotRan);
        if (remainingNeurons > 0)
            Run(_currentNeuron.Children.FirstOrDefault(c => c.NeuronState == NeuronState.NotRan));
        else
            CompleteNeuronBranch();
    }

    public void CompleteNeuronBranch()
    {
        _currentNeuron.NeuronState = NeuronState.Complete;

        var parent = _currentNeuron.Parent;

        if (parent.NeuronType == NeuronType.If)
            parent.NeuronState = NeuronState.Complete;

        Run(parent);
    }
}