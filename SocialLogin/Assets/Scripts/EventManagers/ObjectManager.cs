using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ObjectManager
{
    private static Dictionary<string, ObjectHandler> variablesObject = new Dictionary<string, ObjectHandler>();
    private static Dictionary<string, IntegerHandler> variablesInt = new Dictionary<string, IntegerHandler>();
    private static Dictionary<string, BoolHandler> variablesBool = new Dictionary<string, BoolHandler>();
    private static Dictionary<string, List<ObjectHandler>> masterTableObjects = new Dictionary<string, List<ObjectHandler>>();
    private static Dictionary<string, List<VariableHolderEvent>> variablesEvent = new Dictionary<string, List<VariableHolderEvent>>();


    public static void RegisterCallBackObject(string varName, ObjectHandler.EventHandler callBack)
    {
        if (!variablesObject.ContainsKey(varName))
            variablesObject.Add(varName, new ObjectHandler());
        else if (variablesObject[varName].OnValueChanged != null && variablesObject[varName].OnValueChanged.GetInvocationList().Contains(callBack))
        {
            Debug.LogError("ObjectManager " + callBack.Method.ToString() + "from" + callBack.Target + " is already registered");
        }
        variablesObject[varName].OnValueChanged += callBack;
        if (variablesObject[varName].enableTrace)
            Debug.Log("ObjectManager - Added callback for variable (object)" + varName);

    }

    public static void RegisterCallBackEvent(string varName, VariableHolderEvent.EventHandler callBack, int priority = 0)
    {
        if (!variablesEvent.ContainsKey(varName))
            variablesEvent.Add(varName, new List<VariableHolderEvent>());

        VariableHolderEvent tmp = null;

        for (var i = 0; i < variablesEvent[varName].Count; ++i)
            if (variablesEvent[varName][i].priority == priority)
                tmp = variablesEvent[varName][i];

        if (tmp == null)
        {
            tmp = new VariableHolderEvent
            {
                priority = priority
            };

            variablesEvent[varName].Add(tmp);
            variablesEvent[varName] = variablesEvent[varName].OrderBy(o => o.priority).ToList();
        }

        else if (tmp.OnValueChanged != null && tmp.OnValueChanged.GetInvocationList().Contains(callBack))
        {
            Debug.LogError("ObjectManager " + callBack.Method.ToString() + "from" + callBack.Target + " is already registered");
        }

        tmp.OnValueChanged += callBack;
        if (tmp.enableTrace)
            Debug.Log("ObjectManager - Added callback for variable (object)" + varName);

    }

    public static void Triggerevent(string eventName)
    {
        if (!variablesEvent.ContainsKey(eventName))
            variablesEvent.Add(eventName, new List<VariableHolderEvent>());

        List<VariableHolderEvent> tmp = variablesEvent[eventName];

        for (var i = 0; i < tmp.Count; ++i)
        {
            if (tmp[i].enableTrace)
                Debug.Log("ObjectManager - Launching Event Name =  " + eventName);

            if (tmp[i].OnValueChanged != null)
                Invoke(tmp[i].OnValueChanged.GetInvocationList());
        }
    }

    public static void RegisterCallBackInt(string varName, IntegerHandler.EventHandler callBack)
    {
        if (!variablesObject.ContainsKey(varName))
            variablesInt.Add(varName, new IntegerHandler());
        else if (variablesInt[varName].OnValueChanged != null && variablesInt[varName].OnValueChanged.GetInvocationList().Contains(callBack))
        {
            Debug.LogError("ObjectManager " + callBack.Method.ToString() + "from" + callBack.Target + " is already registered");
        }
        variablesInt[varName].OnValueChanged += callBack;
        if (variablesInt[varName].enableTrace)
            Debug.Log("ObjectManager" + callBack.Method.ToString() + "from" + callBack.Target + "is already registered");

    }

    public static void RegisterCallBackBool(string varName, BoolHandler.EventHandler callBack)
    {
        if (!variablesObject.ContainsKey(varName))
            variablesBool.Add(varName, new BoolHandler());
        else if (variablesBool[varName].OnValueChanged != null && variablesBool[varName].OnValueChanged.GetInvocationList().Contains(callBack))
        {
            Debug.LogError("ObjectManager " + callBack.Method.ToString() + "from" + callBack.Target + " is already registered");
        }
        variablesBool[varName].OnValueChanged += callBack;
        if (variablesBool[varName].enableTrace)
            Debug.Log("ObjectManager" + callBack.Method.ToString() + "from" + callBack.Target + "is already registered");

    }

    public static void UnRegisterCallBackObject(ObjectHandler.EventHandler callBack, string varName = null)
    {
        if (varName != null && variablesObject.ContainsKey(varName))
        {
            if (variablesObject[varName].OnValueChanged != null)
            {
                variablesObject[varName].OnValueChanged -= callBack;

                if (variablesObject[varName].enableTrace)
                    Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);
                return;
            }
        }

        foreach (var item in variablesObject)
        {
            if (item.Value.OnValueChanged == null)
                continue;

            item.Value.OnValueChanged -= callBack;
            if (item.Value.enableTrace)
                Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);

        }
    }

    public static void UnRegisterCallBackInt(IntegerHandler.EventHandler callBack, string varName = null)
    {
        if (varName != null && variablesInt.ContainsKey(varName))
        {
            if (variablesInt[varName].OnValueChanged != null)
            {
                variablesInt[varName].OnValueChanged -= callBack;

                if (variablesInt[varName].enableTrace)
                    Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);
                return;
            }
        }

        foreach (var item in variablesInt)
        {
            if (item.Value.OnValueChanged == null)
                continue;

            item.Value.OnValueChanged -= callBack;
            if (item.Value.enableTrace)
                Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);

        }
    }

    public static void UnRegisterCallBackBool(BoolHandler.EventHandler callBack, string varName = null)
    {
        if (varName != null && variablesBool.ContainsKey(varName))
        {
            if (variablesBool[varName].OnValueChanged != null)
            {
                variablesBool[varName].OnValueChanged -= callBack;

                if (variablesBool[varName].enableTrace)
                    Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);
                return;
            }
        }

        foreach (var item in variablesBool)
        {
            if (item.Value.OnValueChanged == null)
                continue;

            item.Value.OnValueChanged -= callBack;
            if (item.Value.enableTrace)
                Debug.Log("ObjectManager - Removed callback for variable (object)" + varName);

        }
    }

    public static void UnRegisterCallBackEvent(VariableHolderEvent.EventHandler callBack, string varName = null)
    {
        if (varName != null && variablesEvent.ContainsKey(varName))
        {
			for (int i = 0; i < variablesEvent[varName].Count; i++)
            {
				if (variablesEvent[varName][i].OnValueChanged != null && variablesEvent[varName][i].OnValueChanged.GetInvocationList().Contains(callBack))
                {
                    variablesEvent[varName][i].OnValueChanged -= callBack;

                    if (variablesEvent[varName][i].enableTrace)
                        Debug.Log("ObjectManager - Removed callback for variable (Event)" + varName);
                    return;
                }
            }
        }

        foreach (var temp in variablesEvent)
        {
            for (int i = 0; i < temp.Value.Count; i++)
            {
                if (temp.Value[i].OnValueChanged == null)
                    continue;
                temp.Value[i].OnValueChanged -= callBack;
                if(temp.Value[i].enableTrace)
                    Debug.Log("ObjectManager - Removed callback for variable (Event)" + varName);
            }
        }
    }

    public static void SetObject(string varName, object val)
    {
        if (!variablesObject.ContainsKey(varName))
            variablesObject.Add(varName, new ObjectHandler());

        ObjectHandler var = variablesObject[varName];
        if (var.enableTrace)
            Debug.Log("ObjectHandler - Setting variable (Object) - name = " + varName);
        if ((var.value != val) || (var.valueIsAssigned == false))
        {
            var.valueIsAssigned = true;
            var.value = val;
            if (var.OnValueChanged != null)
                Invoke(var.OnValueChanged.GetInvocationList(), var.value);
        }
    }

    public static void SetMultipleObject(string varName, List<object> val)
    {
        if (!masterTableObjects.ContainsKey(varName))
            masterTableObjects.Add(varName, new List<ObjectHandler>());

        List<ObjectHandler> var = masterTableObjects[varName];
        for (int i = 0; i < var.Count; i++)
        {
            if (var[i].enableTrace)
                Debug.Log("ObjectHandler - Setting variable (Object) - name = " + varName);

            if ((var[i].value != val) || (var[i].valueIsAssigned == false))
            {
                var[i].valueIsAssigned = true;
                var[i].value = val;
                if (var[i].OnValueChanged != null)
                    Invoke(var[i].OnValueChanged.GetInvocationList(), var[i].value);
            }
        }
    }

    public static void SetInt(string varName, int val)
    {
        if (!variablesInt.ContainsKey(varName))
            variablesInt.Add(varName, new IntegerHandler());

        IntegerHandler var = variablesInt[varName];
        if (var.enableTrace)
            Debug.Log("ObjectHandler - Setting variable (Object) - name = " + varName);
        if ((var.value != val) || (var.valueIsAssigned == false))
        {
            var.valueIsAssigned = true;
            var.value = val;
            if (var.OnValueChanged != null)
                Invoke(var.OnValueChanged.GetInvocationList(), var.value);
        }
    }

    public static void SetBool(string varName, bool val)
    {
        if (!variablesBool.ContainsKey(varName))
            variablesBool.Add(varName, new BoolHandler());

        BoolHandler var = variablesBool[varName];
        if (var.enableTrace)
            Debug.Log("ObjectHandler - Setting variable (Object) - name = " + varName);
        if ((var.value != val) || (var.valueIsAssigned == false))
        {
            var.valueIsAssigned = true;
            var.value = val;
            if (var.OnValueChanged != null)
                Invoke(var.OnValueChanged.GetInvocationList(), var.value);
        }
    }

    public static object GetObject(string varName)
    {
        object rv = null;
        if (variablesObject.ContainsKey(varName))
            rv = variablesObject[varName].value;
        else
            Debug.LogError("ObjectHandler - does not have a variable (Object) named " + varName + " !");
        return rv;
    }

    public static List<object> GetMultipleObjects(string varName)
    {
        List<object> rv = null;
        if (masterTableObjects.ContainsKey(varName))
            for (int i = 0; i < rv.Count; i++)
            {
                rv[i] = masterTableObjects[varName][i].value;
            }
        else
            Debug.LogError("ObjectHandler - does not have a variable (Object) named " + varName + " !");
        return rv;
    }

    public static int GetInt(string varName)
    {
        int rv = -1;
        if (variablesInt.ContainsKey(varName))
            rv = variablesInt[varName].value;
        else
            Debug.LogError("IntHandler - does not have a variable (int) named " + varName + " !");
        return rv;
    }

    public static bool GetBool(string varName)
    {
        bool rv = false;
        if (variablesBool.ContainsKey(varName))
            rv = variablesBool[varName].value;
        else
            Debug.LogError("IntHandler - does not have a variable (int) named " + varName + " !");
        return rv;
    }

    private static void Invoke(Delegate[] list, params object[] args)
    {
        for (int i = 0; i < list.Length; ++i)
        {
            bool enabled = true;

            if (enabled)
                list[i].DynamicInvoke(args);
        }
    }
}

public class ObjectHandlerBase
{
    public bool enableTrace = false;
    public bool valueIsAssigned = false;
}

public class ObjectHandler : ObjectHandlerBase
{
    public delegate void EventHandler(object param);
    public EventHandler OnValueChanged = null;
    public object value;
}

public class IntegerHandler : ObjectHandlerBase
{
    public delegate void EventHandler(int param);
    public EventHandler OnValueChanged = null;
    public int value;
}

public class BoolHandler : ObjectHandlerBase
{
    public delegate void EventHandler(bool param);
    public EventHandler OnValueChanged = null;
    public bool value;
}

public class VariableHolderEvent : ObjectHandlerBase
{
    public int priority;
    public delegate void EventHandler();
    public EventHandler OnValueChanged = null;
}