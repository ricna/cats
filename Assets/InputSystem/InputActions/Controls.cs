//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.2
//     from Assets/InputSystem/InputActions/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Cat"",
            ""id"": ""e7f033d9-29f5-4209-b8de-7593c29fdb15"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6adb314b-f27f-446d-b853-bc898fec234c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1f97a621-3d03-44ba-8036-12d3cbda40db"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""eed07ad4-4446-4947-823b-c40d0da09cc7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""a51a22b3-3930-4eb3-b40e-2ecc58fa3d41"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability01"",
                    ""type"": ""Button"",
                    ""id"": ""d4c1cdb0-32be-47c3-89c9-778776e4cded"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability02"",
                    ""type"": ""Button"",
                    ""id"": ""dc85e87c-b22c-4a10-9579-4dbd1e33710a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability03"",
                    ""type"": ""Button"",
                    ""id"": ""0f40dfce-7788-4e44-be86-e4bbff3b7790"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability04"",
                    ""type"": ""Button"",
                    ""id"": ""d92c1e4e-c708-482e-b9e6-8f0f73467c24"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Minimap"",
                    ""type"": ""Button"",
                    ""id"": ""9fe1d713-6d04-4dd0-8ff3-519aa6be0fc6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleMenu"",
                    ""type"": ""Button"",
                    ""id"": ""886909c2-7408-47eb-ad75-e9581809bf7f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6efa539b-4448-4d44-9d21-d4d79ca717bc"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Ability01"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""584b1bf7-fdb5-4085-b3f8-f7ec3a917d93"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1f491a26-615a-485b-9986-99021ec309bf"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cb912902-bf9e-4de5-b565-fd770ac602a5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ee657c6-8fb1-4794-9061-514db5cce78f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8f61a756-496e-42e0-817e-a15879f4484c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3e68912f-b37e-4008-90ca-b4a41a5ca975"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe273dad-9c2b-46c3-a1b5-4c330cf7978f"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""670907f2-a79d-43fc-b355-8faf8ed1598d"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Ability02"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""422d8263-d04b-48cf-9701-f928b30829d8"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Ability03"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe14f600-390c-4bed-8654-54c66d5872ff"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Ability04"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c96ed6d2-5aa5-4d8c-8fde-9dea1ca2386c"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""439185b7-f692-459d-93d5-c44bf99c6871"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2f77951-41b8-4803-9d44-fe9411b541c3"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5da8fd9d-74b7-4b6d-a391-7db82a449344"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05c53e9c-4394-40b2-8c9c-3dcba5fd72b8"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e4744a1-c8e0-4b73-af33-f77672e7ad1f"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be64ea6e-ba56-43d6-ad67-81cc5063a0d3"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6a18e71-bdfb-48c1-be3e-e54fb0fe8068"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff2d0d0c-8694-4467-a6a1-dfef073728fa"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9423508-e20b-4287-b03e-310133784f5e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""Minimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""604b6fa6-ebf6-4957-96b7-437402b5c5b9"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""ToggleMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c614629d-79c7-4c07-9519-7f98ce84c582"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard"",
                    ""action"": ""ToggleMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Phone"",
            ""bindingGroup"": ""Phone"",
            ""devices"": []
        }
    ]
}");
        // Cat
        m_Cat = asset.FindActionMap("Cat", throwIfNotFound: true);
        m_Cat_Move = m_Cat.FindAction("Move", throwIfNotFound: true);
        m_Cat_Interact = m_Cat.FindAction("Interact", throwIfNotFound: true);
        m_Cat_Sprint = m_Cat.FindAction("Sprint", throwIfNotFound: true);
        m_Cat_Crouch = m_Cat.FindAction("Crouch", throwIfNotFound: true);
        m_Cat_Ability01 = m_Cat.FindAction("Ability01", throwIfNotFound: true);
        m_Cat_Ability02 = m_Cat.FindAction("Ability02", throwIfNotFound: true);
        m_Cat_Ability03 = m_Cat.FindAction("Ability03", throwIfNotFound: true);
        m_Cat_Ability04 = m_Cat.FindAction("Ability04", throwIfNotFound: true);
        m_Cat_Minimap = m_Cat.FindAction("Minimap", throwIfNotFound: true);
        m_Cat_ToggleMenu = m_Cat.FindAction("ToggleMenu", throwIfNotFound: true);
    }

    ~@Controls()
    {
        Debug.Assert(!m_Cat.enabled, "This will cause a leak and performance issues, Controls.Cat.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Cat
    private readonly InputActionMap m_Cat;
    private List<ICatActions> m_CatActionsCallbackInterfaces = new List<ICatActions>();
    private readonly InputAction m_Cat_Move;
    private readonly InputAction m_Cat_Interact;
    private readonly InputAction m_Cat_Sprint;
    private readonly InputAction m_Cat_Crouch;
    private readonly InputAction m_Cat_Ability01;
    private readonly InputAction m_Cat_Ability02;
    private readonly InputAction m_Cat_Ability03;
    private readonly InputAction m_Cat_Ability04;
    private readonly InputAction m_Cat_Minimap;
    private readonly InputAction m_Cat_ToggleMenu;
    public struct CatActions
    {
        private @Controls m_Wrapper;
        public CatActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Cat_Move;
        public InputAction @Interact => m_Wrapper.m_Cat_Interact;
        public InputAction @Sprint => m_Wrapper.m_Cat_Sprint;
        public InputAction @Crouch => m_Wrapper.m_Cat_Crouch;
        public InputAction @Ability01 => m_Wrapper.m_Cat_Ability01;
        public InputAction @Ability02 => m_Wrapper.m_Cat_Ability02;
        public InputAction @Ability03 => m_Wrapper.m_Cat_Ability03;
        public InputAction @Ability04 => m_Wrapper.m_Cat_Ability04;
        public InputAction @Minimap => m_Wrapper.m_Cat_Minimap;
        public InputAction @ToggleMenu => m_Wrapper.m_Cat_ToggleMenu;
        public InputActionMap Get() { return m_Wrapper.m_Cat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CatActions set) { return set.Get(); }
        public void AddCallbacks(ICatActions instance)
        {
            if (instance == null || m_Wrapper.m_CatActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CatActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Ability01.started += instance.OnAbility01;
            @Ability01.performed += instance.OnAbility01;
            @Ability01.canceled += instance.OnAbility01;
            @Ability02.started += instance.OnAbility02;
            @Ability02.performed += instance.OnAbility02;
            @Ability02.canceled += instance.OnAbility02;
            @Ability03.started += instance.OnAbility03;
            @Ability03.performed += instance.OnAbility03;
            @Ability03.canceled += instance.OnAbility03;
            @Ability04.started += instance.OnAbility04;
            @Ability04.performed += instance.OnAbility04;
            @Ability04.canceled += instance.OnAbility04;
            @Minimap.started += instance.OnMinimap;
            @Minimap.performed += instance.OnMinimap;
            @Minimap.canceled += instance.OnMinimap;
            @ToggleMenu.started += instance.OnToggleMenu;
            @ToggleMenu.performed += instance.OnToggleMenu;
            @ToggleMenu.canceled += instance.OnToggleMenu;
        }

        private void UnregisterCallbacks(ICatActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Ability01.started -= instance.OnAbility01;
            @Ability01.performed -= instance.OnAbility01;
            @Ability01.canceled -= instance.OnAbility01;
            @Ability02.started -= instance.OnAbility02;
            @Ability02.performed -= instance.OnAbility02;
            @Ability02.canceled -= instance.OnAbility02;
            @Ability03.started -= instance.OnAbility03;
            @Ability03.performed -= instance.OnAbility03;
            @Ability03.canceled -= instance.OnAbility03;
            @Ability04.started -= instance.OnAbility04;
            @Ability04.performed -= instance.OnAbility04;
            @Ability04.canceled -= instance.OnAbility04;
            @Minimap.started -= instance.OnMinimap;
            @Minimap.performed -= instance.OnMinimap;
            @Minimap.canceled -= instance.OnMinimap;
            @ToggleMenu.started -= instance.OnToggleMenu;
            @ToggleMenu.performed -= instance.OnToggleMenu;
            @ToggleMenu.canceled -= instance.OnToggleMenu;
        }

        public void RemoveCallbacks(ICatActions instance)
        {
            if (m_Wrapper.m_CatActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICatActions instance)
        {
            foreach (var item in m_Wrapper.m_CatActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CatActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CatActions @Cat => new CatActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_PhoneSchemeIndex = -1;
    public InputControlScheme PhoneScheme
    {
        get
        {
            if (m_PhoneSchemeIndex == -1) m_PhoneSchemeIndex = asset.FindControlSchemeIndex("Phone");
            return asset.controlSchemes[m_PhoneSchemeIndex];
        }
    }
    public interface ICatActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnAbility01(InputAction.CallbackContext context);
        void OnAbility02(InputAction.CallbackContext context);
        void OnAbility03(InputAction.CallbackContext context);
        void OnAbility04(InputAction.CallbackContext context);
        void OnMinimap(InputAction.CallbackContext context);
        void OnToggleMenu(InputAction.CallbackContext context);
    }
}
