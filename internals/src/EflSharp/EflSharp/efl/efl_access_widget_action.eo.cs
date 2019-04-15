#pragma warning disable CS1591
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
namespace Efl { namespace Access { namespace Widget { 
/// <summary>Access widget action mixin</summary>
[IActionNativeInherit]
public interface IAction : 
    Efl.Access.IAction ,
    Efl.Eo.IWrapper, IDisposable
{
    /// <summary>Elementary actions</summary>
/// <returns>NULL-terminated array of Efl.Access.Action_Data.</returns>
Efl.Access.ActionData GetElmActions();
        /// <summary></summary>
    event EventHandler ReadingStateChangedEvt;
    /// <summary>Elementary actions</summary>
/// <value>NULL-terminated array of Efl.Access.Action_Data.</value>
    Efl.Access.ActionData ElmActions {
        get ;
    }
}
/// <summary>Access widget action mixin</summary>
sealed public class IActionConcrete : 

IAction
    , Efl.Access.IAction
{
    ///<summary>Pointer to the native class description.</summary>
    public System.IntPtr NativeClass {
        get {
            if (((object)this).GetType() == typeof (IActionConcrete))
                return Efl.Access.Widget.IActionNativeInherit.GetEflClassStatic();
            else
                return Efl.Eo.ClassRegister.klassFromType[((object)this).GetType()];
        }
    }
    private EventHandlerList eventHandlers = new EventHandlerList();
    private  System.IntPtr handle;
    ///<summary>Pointer to the native instance.</summary>
    public System.IntPtr NativeHandle {
        get { return handle; }
    }
    [System.Runtime.InteropServices.DllImport(efl.Libs.Elementary)] internal static extern System.IntPtr
        efl_access_widget_action_mixin_get();
    ///<summary>Internal usage: Constructs an instance from a native pointer. This is used when interacting with C code and should not be used directly.</summary>
    private IActionConcrete(System.IntPtr raw)
    {
        handle = raw;
        RegisterEventProxies();
    }
    ///<summary>Destructor.</summary>
    ~IActionConcrete()
    {
        Dispose(false);
    }
    ///<summary>Releases the underlying native instance.</summary>
    void Dispose(bool disposing)
    {
        if (handle != System.IntPtr.Zero) {
            Efl.Eo.Globals.efl_unref(handle);
            handle = System.IntPtr.Zero;
        }
    }
    ///<summary>Releases the underlying native instance.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ///<summary>Verifies if the given object is equal to this one.</summary>
    public override bool Equals(object obj)
    {
        var other = obj as Efl.Object;
        if (other == null)
            return false;
        return this.NativeHandle == other.NativeHandle;
    }
    ///<summary>Gets the hash code for this object based on the native pointer it points to.</summary>
    public override int GetHashCode()
    {
        return this.NativeHandle.ToInt32();
    }
    ///<summary>Turns the native pointer into a string representation.</summary>
    public override String ToString()
    {
        return $"{this.GetType().Name}@[{this.NativeHandle.ToInt32():x}]";
    }
    private readonly object eventLock = new object();
    private Dictionary<string, int> event_cb_count = new Dictionary<string, int>();
    ///<summary>Adds a new event handler, registering it to the native event. For internal use only.</summary>
    ///<param name="lib">The name of the native library definining the event.</param>
    ///<param name="key">The name of the native event.</param>
    ///<param name="evt_delegate">The delegate to be called on event raising.</param>
    ///<returns>True if the delegate was successfully registered.</returns>
    private bool AddNativeEventHandler(string lib, string key, Efl.EventCb evt_delegate) {
        int event_count = 0;
        if (!event_cb_count.TryGetValue(key, out event_count))
            event_cb_count[key] = event_count;
        if (event_count == 0) {
            IntPtr desc = Efl.EventDescription.GetNative(lib, key);
            if (desc == IntPtr.Zero) {
                Eina.Log.Error($"Failed to get native event {key}");
                return false;
            }
             bool result = Efl.Eo.Globals.efl_event_callback_priority_add(handle, desc, 0, evt_delegate, System.IntPtr.Zero);
            if (!result) {
                Eina.Log.Error($"Failed to add event proxy for event {key}");
                return false;
            }
            Eina.Error.RaiseIfUnhandledException();
        } 
        event_cb_count[key]++;
        return true;
    }
    ///<summary>Removes the given event handler for the given event. For internal use only.</summary>
    ///<param name="key">The name of the native event.</param>
    ///<param name="evt_delegate">The delegate to be removed.</param>
    ///<returns>True if the delegate was successfully registered.</returns>
    private bool RemoveNativeEventHandler(string key, Efl.EventCb evt_delegate) {
        int event_count = 0;
        if (!event_cb_count.TryGetValue(key, out event_count))
            event_cb_count[key] = event_count;
        if (event_count == 1) {
            IntPtr desc = Efl.EventDescription.GetNative(efl.Libs.Elementary, key);
            if (desc == IntPtr.Zero) {
                Eina.Log.Error($"Failed to get native event {key}");
                return false;
            }
            bool result = Efl.Eo.Globals.efl_event_callback_del(handle, desc, evt_delegate, System.IntPtr.Zero);
            if (!result) {
                Eina.Log.Error($"Failed to remove event proxy for event {key}");
                return false;
            }
            Eina.Error.RaiseIfUnhandledException();
        } else if (event_count == 0) {
            Eina.Log.Error($"Trying to remove proxy for event {key} when there is nothing registered.");
            return false;
        } 
        event_cb_count[key]--;
        return true;
    }
private static object ReadingStateChangedEvtKey = new object();
    /// <summary></summary>
    public event EventHandler ReadingStateChangedEvt
    {
        add {
            lock (eventLock) {
                string key = "_EFL_ACCESS_WIDGET_ACTION_EVENT_READING_STATE_CHANGED";
                if (AddNativeEventHandler(efl.Libs.Elementary, key, this.evt_ReadingStateChangedEvt_delegate)) {
                    eventHandlers.AddHandler(ReadingStateChangedEvtKey , value);
                } else
                    Eina.Log.Error($"Error adding proxy for event {key}");
            }
        }
        remove {
            lock (eventLock) {
                string key = "_EFL_ACCESS_WIDGET_ACTION_EVENT_READING_STATE_CHANGED";
                if (RemoveNativeEventHandler(key, this.evt_ReadingStateChangedEvt_delegate)) { 
                    eventHandlers.RemoveHandler(ReadingStateChangedEvtKey , value);
                } else
                    Eina.Log.Error($"Error removing proxy for event {key}");
            }
        }
    }
    ///<summary>Method to raise event ReadingStateChangedEvt.</summary>
    public void On_ReadingStateChangedEvt(EventArgs e)
    {
        EventHandler evt;
        lock (eventLock) {
        evt = (EventHandler)eventHandlers[ReadingStateChangedEvtKey];
        }
        evt?.Invoke(this, e);
    }
    Efl.EventCb evt_ReadingStateChangedEvt_delegate;
    private void on_ReadingStateChangedEvt_NativeCallback(System.IntPtr data, ref Efl.Event.NativeStruct evt)
    {
        EventArgs args = EventArgs.Empty;
        try {
            On_ReadingStateChangedEvt(args);
        } catch (Exception e) {
            Eina.Log.Error(e.ToString());
            Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
        }
    }

    ///<summary>Register the Eo event wrappers making the bridge to C# events. Internal usage only.</summary>
     void RegisterEventProxies()
    {
        evt_ReadingStateChangedEvt_delegate = new Efl.EventCb(on_ReadingStateChangedEvt_NativeCallback);
    }
    /// <summary>Elementary actions</summary>
    /// <returns>NULL-terminated array of Efl.Access.Action_Data.</returns>
    public Efl.Access.ActionData GetElmActions() {
         var _ret_var = Efl.Access.Widget.IActionNativeInherit.efl_access_widget_action_elm_actions_get_ptr.Value.Delegate(this.NativeHandle);
        Eina.Error.RaiseIfUnhandledException();
        return _ret_var;
 }
    /// <summary>Gets action name for given id</summary>
    /// <param name="id">ID to get action name for</param>
    /// <returns>Action name</returns>
    public System.String GetActionName( int id) {
                                 var _ret_var = Efl.Access.IActionNativeInherit.efl_access_action_name_get_ptr.Value.Delegate(this.NativeHandle, id);
        Eina.Error.RaiseIfUnhandledException();
                        return _ret_var;
 }
    /// <summary>Gets localized action name for given id</summary>
    /// <param name="id">ID to get localized name for</param>
    /// <returns>Localized name</returns>
    public System.String GetActionLocalizedName( int id) {
                                 var _ret_var = Efl.Access.IActionNativeInherit.efl_access_action_localized_name_get_ptr.Value.Delegate(this.NativeHandle, id);
        Eina.Error.RaiseIfUnhandledException();
                        return _ret_var;
 }
    /// <summary>Get list of available widget actions</summary>
    /// <returns>Contains statically allocated strings.</returns>
    public Eina.List<Efl.Access.ActionData> GetActions() {
         var _ret_var = Efl.Access.IActionNativeInherit.efl_access_action_actions_get_ptr.Value.Delegate(this.NativeHandle);
        Eina.Error.RaiseIfUnhandledException();
        return new Eina.List<Efl.Access.ActionData>(_ret_var, false, false);
 }
    /// <summary>Performs action on given widget.</summary>
    /// <param name="id">ID for widget</param>
    /// <returns><c>true</c> if action was performed, <c>false</c> otherwise</returns>
    public bool ActionDo( int id) {
                                 var _ret_var = Efl.Access.IActionNativeInherit.efl_access_action_do_ptr.Value.Delegate(this.NativeHandle, id);
        Eina.Error.RaiseIfUnhandledException();
                        return _ret_var;
 }
    /// <summary>Gets configured keybinding for specific action and widget.</summary>
    /// <param name="id">ID for widget</param>
    /// <returns>Should be freed by the user.</returns>
    public System.String GetActionKeybinding( int id) {
                                 var _ret_var = Efl.Access.IActionNativeInherit.efl_access_action_keybinding_get_ptr.Value.Delegate(this.NativeHandle, id);
        Eina.Error.RaiseIfUnhandledException();
                        return _ret_var;
 }
    /// <summary>Elementary actions</summary>
/// <value>NULL-terminated array of Efl.Access.Action_Data.</value>
    public Efl.Access.ActionData ElmActions {
        get { return GetElmActions(); }
    }
    /// <summary>Get list of available widget actions</summary>
/// <value>Contains statically allocated strings.</value>
    public Eina.List<Efl.Access.ActionData> Actions {
        get { return GetActions(); }
    }
    private static IntPtr GetEflClassStatic()
    {
        return Efl.Access.Widget.IActionConcrete.efl_access_widget_action_mixin_get();
    }
}
public class IActionNativeInherit  : Efl.Eo.NativeClass{
    public  static Efl.Eo.NativeModule _Module = new Efl.Eo.NativeModule(efl.Libs.Elementary);
    public override System.Collections.Generic.List<Efl_Op_Description> GetEoOps(System.Type type)
    {
        var descs = new System.Collections.Generic.List<Efl_Op_Description>();
        var methods = Efl.Eo.Globals.GetUserMethods(type);
        if (efl_access_widget_action_elm_actions_get_static_delegate == null)
            efl_access_widget_action_elm_actions_get_static_delegate = new efl_access_widget_action_elm_actions_get_delegate(elm_actions_get);
        if (methods.FirstOrDefault(m => m.Name == "GetElmActions") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_widget_action_elm_actions_get"), func = Marshal.GetFunctionPointerForDelegate(efl_access_widget_action_elm_actions_get_static_delegate)});
        if (efl_access_action_name_get_static_delegate == null)
            efl_access_action_name_get_static_delegate = new efl_access_action_name_get_delegate(action_name_get);
        if (methods.FirstOrDefault(m => m.Name == "GetActionName") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_action_name_get"), func = Marshal.GetFunctionPointerForDelegate(efl_access_action_name_get_static_delegate)});
        if (efl_access_action_localized_name_get_static_delegate == null)
            efl_access_action_localized_name_get_static_delegate = new efl_access_action_localized_name_get_delegate(action_localized_name_get);
        if (methods.FirstOrDefault(m => m.Name == "GetActionLocalizedName") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_action_localized_name_get"), func = Marshal.GetFunctionPointerForDelegate(efl_access_action_localized_name_get_static_delegate)});
        if (efl_access_action_actions_get_static_delegate == null)
            efl_access_action_actions_get_static_delegate = new efl_access_action_actions_get_delegate(actions_get);
        if (methods.FirstOrDefault(m => m.Name == "GetActions") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_action_actions_get"), func = Marshal.GetFunctionPointerForDelegate(efl_access_action_actions_get_static_delegate)});
        if (efl_access_action_do_static_delegate == null)
            efl_access_action_do_static_delegate = new efl_access_action_do_delegate(action_do);
        if (methods.FirstOrDefault(m => m.Name == "ActionDo") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_action_do"), func = Marshal.GetFunctionPointerForDelegate(efl_access_action_do_static_delegate)});
        if (efl_access_action_keybinding_get_static_delegate == null)
            efl_access_action_keybinding_get_static_delegate = new efl_access_action_keybinding_get_delegate(action_keybinding_get);
        if (methods.FirstOrDefault(m => m.Name == "GetActionKeybinding") != null)
            descs.Add(new Efl_Op_Description() {api_func = Efl.Eo.FunctionInterop.LoadFunctionPointer(_Module.Module, "efl_access_action_keybinding_get"), func = Marshal.GetFunctionPointerForDelegate(efl_access_action_keybinding_get_static_delegate)});
        return descs;
    }
    public override IntPtr GetEflClass()
    {
        return Efl.Access.Widget.IActionConcrete.efl_access_widget_action_mixin_get();
    }
    public static  IntPtr GetEflClassStatic()
    {
        return Efl.Access.Widget.IActionConcrete.efl_access_widget_action_mixin_get();
    }


     private delegate Efl.Access.ActionData efl_access_widget_action_elm_actions_get_delegate(System.IntPtr obj, System.IntPtr pd);


     public delegate Efl.Access.ActionData efl_access_widget_action_elm_actions_get_api_delegate(System.IntPtr obj);
     public static Efl.Eo.FunctionWrapper<efl_access_widget_action_elm_actions_get_api_delegate> efl_access_widget_action_elm_actions_get_ptr = new Efl.Eo.FunctionWrapper<efl_access_widget_action_elm_actions_get_api_delegate>(_Module, "efl_access_widget_action_elm_actions_get");
     private static Efl.Access.ActionData elm_actions_get(System.IntPtr obj, System.IntPtr pd)
    {
        Eina.Log.Debug("function efl_access_widget_action_elm_actions_get was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                        Efl.Access.ActionData _ret_var = default(Efl.Access.ActionData);
            try {
                _ret_var = ((IActionConcrete)wrapper).GetElmActions();
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
        return _ret_var;
        } else {
            return efl_access_widget_action_elm_actions_get_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)));
        }
    }
    private static efl_access_widget_action_elm_actions_get_delegate efl_access_widget_action_elm_actions_get_static_delegate;


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringKeepOwnershipMarshaler))] private delegate System.String efl_access_action_name_get_delegate(System.IntPtr obj, System.IntPtr pd,   int id);


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringKeepOwnershipMarshaler))] public delegate System.String efl_access_action_name_get_api_delegate(System.IntPtr obj,   int id);
     public static Efl.Eo.FunctionWrapper<efl_access_action_name_get_api_delegate> efl_access_action_name_get_ptr = new Efl.Eo.FunctionWrapper<efl_access_action_name_get_api_delegate>(_Module, "efl_access_action_name_get");
     private static System.String action_name_get(System.IntPtr obj, System.IntPtr pd,  int id)
    {
        Eina.Log.Debug("function efl_access_action_name_get was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                                                System.String _ret_var = default(System.String);
            try {
                _ret_var = ((IActionConcrete)wrapper).GetActionName( id);
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
                        return _ret_var;
        } else {
            return efl_access_action_name_get_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)),  id);
        }
    }
    private static efl_access_action_name_get_delegate efl_access_action_name_get_static_delegate;


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringKeepOwnershipMarshaler))] private delegate System.String efl_access_action_localized_name_get_delegate(System.IntPtr obj, System.IntPtr pd,   int id);


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringKeepOwnershipMarshaler))] public delegate System.String efl_access_action_localized_name_get_api_delegate(System.IntPtr obj,   int id);
     public static Efl.Eo.FunctionWrapper<efl_access_action_localized_name_get_api_delegate> efl_access_action_localized_name_get_ptr = new Efl.Eo.FunctionWrapper<efl_access_action_localized_name_get_api_delegate>(_Module, "efl_access_action_localized_name_get");
     private static System.String action_localized_name_get(System.IntPtr obj, System.IntPtr pd,  int id)
    {
        Eina.Log.Debug("function efl_access_action_localized_name_get was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                                                System.String _ret_var = default(System.String);
            try {
                _ret_var = ((IActionConcrete)wrapper).GetActionLocalizedName( id);
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
                        return _ret_var;
        } else {
            return efl_access_action_localized_name_get_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)),  id);
        }
    }
    private static efl_access_action_localized_name_get_delegate efl_access_action_localized_name_get_static_delegate;


     private delegate System.IntPtr efl_access_action_actions_get_delegate(System.IntPtr obj, System.IntPtr pd);


     public delegate System.IntPtr efl_access_action_actions_get_api_delegate(System.IntPtr obj);
     public static Efl.Eo.FunctionWrapper<efl_access_action_actions_get_api_delegate> efl_access_action_actions_get_ptr = new Efl.Eo.FunctionWrapper<efl_access_action_actions_get_api_delegate>(_Module, "efl_access_action_actions_get");
     private static System.IntPtr actions_get(System.IntPtr obj, System.IntPtr pd)
    {
        Eina.Log.Debug("function efl_access_action_actions_get was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                        Eina.List<Efl.Access.ActionData> _ret_var = default(Eina.List<Efl.Access.ActionData>);
            try {
                _ret_var = ((IActionConcrete)wrapper).GetActions();
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
        return _ret_var.Handle;
        } else {
            return efl_access_action_actions_get_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)));
        }
    }
    private static efl_access_action_actions_get_delegate efl_access_action_actions_get_static_delegate;


     [return: MarshalAs(UnmanagedType.U1)] private delegate bool efl_access_action_do_delegate(System.IntPtr obj, System.IntPtr pd,   int id);


     [return: MarshalAs(UnmanagedType.U1)] public delegate bool efl_access_action_do_api_delegate(System.IntPtr obj,   int id);
     public static Efl.Eo.FunctionWrapper<efl_access_action_do_api_delegate> efl_access_action_do_ptr = new Efl.Eo.FunctionWrapper<efl_access_action_do_api_delegate>(_Module, "efl_access_action_do");
     private static bool action_do(System.IntPtr obj, System.IntPtr pd,  int id)
    {
        Eina.Log.Debug("function efl_access_action_do was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                                                bool _ret_var = default(bool);
            try {
                _ret_var = ((IActionConcrete)wrapper).ActionDo( id);
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
                        return _ret_var;
        } else {
            return efl_access_action_do_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)),  id);
        }
    }
    private static efl_access_action_do_delegate efl_access_action_do_static_delegate;


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringPassOwnershipMarshaler))] private delegate System.String efl_access_action_keybinding_get_delegate(System.IntPtr obj, System.IntPtr pd,   int id);


     [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(Efl.Eo.StringPassOwnershipMarshaler))] public delegate System.String efl_access_action_keybinding_get_api_delegate(System.IntPtr obj,   int id);
     public static Efl.Eo.FunctionWrapper<efl_access_action_keybinding_get_api_delegate> efl_access_action_keybinding_get_ptr = new Efl.Eo.FunctionWrapper<efl_access_action_keybinding_get_api_delegate>(_Module, "efl_access_action_keybinding_get");
     private static System.String action_keybinding_get(System.IntPtr obj, System.IntPtr pd,  int id)
    {
        Eina.Log.Debug("function efl_access_action_keybinding_get was called");
        Efl.Eo.IWrapper wrapper = Efl.Eo.Globals.PrivateDataGet(pd);
        if(wrapper != null) {
                                                System.String _ret_var = default(System.String);
            try {
                _ret_var = ((IActionConcrete)wrapper).GetActionKeybinding( id);
            } catch (Exception e) {
                Eina.Log.Warning($"Callback error: {e.ToString()}");
                Eina.Error.Set(Eina.Error.UNHANDLED_EXCEPTION);
            }
                        return _ret_var;
        } else {
            return efl_access_action_keybinding_get_ptr.Value.Delegate(Efl.Eo.Globals.efl_super(obj, Efl.Eo.Globals.efl_class_get(obj)),  id);
        }
    }
    private static efl_access_action_keybinding_get_delegate efl_access_action_keybinding_get_static_delegate;
}
} } } 
