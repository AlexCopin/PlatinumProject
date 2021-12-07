// GENERATED AUTOMATICALLY FROM 'Assets/StickIt/Scripts/Input/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""NormalInputs"",
            ""id"": ""55b1d386-ead8-4d59-b3b6-320d9db689de"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c1a302f3-670e-4b70-8fe3-42f056ed090b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Direction"",
                    ""type"": ""Value"",
                    ""id"": ""b16cc044-ba91-485f-a9b1-3a9a667baf49"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""d6731089-5380-4b24-a009-c1343ba1c4d1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""b0ad7773-f602-4180-bfda-02abba2ebfd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Test"",
                    ""type"": ""Button"",
                    ""id"": ""b9b77ec6-9fc8-4ad8-a2f8-fcf304d35387"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Test1"",
                    ""type"": ""Button"",
                    ""id"": ""eb04e253-26f8-4f39-abd6-2de47bfaec10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Test2"",
                    ""type"": ""Button"",
                    ""id"": ""c604141d-c625-454e-8327-c75afb25a057"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Test3"",
                    ""type"": ""Button"",
                    ""id"": ""420328b5-a567-4224-b208-a5a36f816c60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""52281ed5-70c9-4610-801d-a6958c8f28ff"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93e22bf6-7b1a-4099-9889-61237ed77478"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e669e173-b6da-40ec-ab8c-39008d1f44c4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1ce3d64-5943-4623-b6bb-3335016c4fde"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3dc3c78b-c892-4289-b41b-e31cc1b813fd"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Keyboard"",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1c24fb3-58ad-419d-8e3a-cad8c4b9f8de"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fabbeff4-dbd9-410d-a731-55bc1d204b37"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Test1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f1cb0ab-63ed-48e9-8857-8fd44b9fd0d1"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Test2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc7a9d13-cf3b-4537-876f-3e99864c8783"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Test3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""1ab45db5-87ed-4028-b279-bb8fef77cc1c"",
            ""actions"": [
                {
                    ""name"": ""Return"",
                    ""type"": ""Button"",
                    ""id"": ""d899757b-19de-4aa5-b97e-e4e9451fda29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a0e037f6-2ae6-4d18-9967-23e1225a3bb3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""3ff5497e-0b95-4bcd-9cc8-79627a38bb74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""827cc017-87a6-4b7b-b4ba-f6028add973a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9ec0de42-398a-46b1-8f2f-35e5f68acab3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a81b5830-73e4-4a8e-b0e8-dac74f836969"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0f45a43-9135-4174-9744-5059b8d384c3"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""c51a0733-f625-4bd9-bfb1-2fd1b46ab4b3"",
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
                    ""id"": ""14eae6f6-6aa9-4adc-a330-3bbbc7ddc5eb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5e75d03f-5687-4f3f-b198-4eb50df188b5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""13d31fd2-fdab-477d-b288-ee396292c108"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""47f0eacd-08af-4a5c-8a8e-083f92de8462"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""842b10d3-0bb0-45ac-a822-829a12d5addf"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""84f301b8-4c2c-4698-8ac4-982d0527968f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ebfd39fd-bdc0-46a9-8fdf-2cd69da6d4e8"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0c4557b6-3f58-408a-a684-0cd26a568b9d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""5030ea9f-1625-4cf6-840e-5ac8c67c2ebb"",
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
                    ""id"": ""2d54ac23-0134-4938-94c5-4aecddc7c2ec"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""715b93a1-1aad-40a7-b6db-230413043abb"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c1f50a03-168c-4a30-b441-9078ea6d416b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""395e18f1-09d2-4513-b714-55fa9622bd46"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""RightStick"",
                    ""id"": ""cdd9e060-8d11-4dcf-bdbd-70353e7ac02d"",
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
                    ""id"": ""6eebbaa4-a0bd-43a0-80ac-54240569b1a2"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""652aa1e8-ac2d-47df-b8c2-15ecbd6e4083"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""88038ba2-6af9-46ee-be2f-88aaed65f066"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4562f19f-0b70-422d-9399-8177a5752275"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0ec79631-19d3-46a0-a095-01a7e09a06f6"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8586efb-706d-43fe-a61a-945bcbc68348"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // NormalInputs
        m_NormalInputs = asset.FindActionMap("NormalInputs", throwIfNotFound: true);
        m_NormalInputs_Jump = m_NormalInputs.FindAction("Jump", throwIfNotFound: true);
        m_NormalInputs_Direction = m_NormalInputs.FindAction("Direction", throwIfNotFound: true);
        m_NormalInputs_MousePosition = m_NormalInputs.FindAction("MousePosition", throwIfNotFound: true);
        m_NormalInputs_Quit = m_NormalInputs.FindAction("Quit", throwIfNotFound: true);
        m_NormalInputs_Test = m_NormalInputs.FindAction("Test", throwIfNotFound: true);
        m_NormalInputs_Test1 = m_NormalInputs.FindAction("Test1", throwIfNotFound: true);
        m_NormalInputs_Test2 = m_NormalInputs.FindAction("Test2", throwIfNotFound: true);
        m_NormalInputs_Test3 = m_NormalInputs.FindAction("Test3", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Return = m_UI.FindAction("Return", throwIfNotFound: true);
        m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
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

    // NormalInputs
    private readonly InputActionMap m_NormalInputs;
    private INormalInputsActions m_NormalInputsActionsCallbackInterface;
    private readonly InputAction m_NormalInputs_Jump;
    private readonly InputAction m_NormalInputs_Direction;
    private readonly InputAction m_NormalInputs_MousePosition;
    private readonly InputAction m_NormalInputs_Quit;
    private readonly InputAction m_NormalInputs_Test;
    private readonly InputAction m_NormalInputs_Test1;
    private readonly InputAction m_NormalInputs_Test2;
    private readonly InputAction m_NormalInputs_Test3;
    public struct NormalInputsActions
    {
        private @PlayerInputs m_Wrapper;
        public NormalInputsActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_NormalInputs_Jump;
        public InputAction @Direction => m_Wrapper.m_NormalInputs_Direction;
        public InputAction @MousePosition => m_Wrapper.m_NormalInputs_MousePosition;
        public InputAction @Quit => m_Wrapper.m_NormalInputs_Quit;
        public InputAction @Test => m_Wrapper.m_NormalInputs_Test;
        public InputAction @Test1 => m_Wrapper.m_NormalInputs_Test1;
        public InputAction @Test2 => m_Wrapper.m_NormalInputs_Test2;
        public InputAction @Test3 => m_Wrapper.m_NormalInputs_Test3;
        public InputActionMap Get() { return m_Wrapper.m_NormalInputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NormalInputsActions set) { return set.Get(); }
        public void SetCallbacks(INormalInputsActions instance)
        {
            if (m_Wrapper.m_NormalInputsActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnJump;
                @Direction.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnDirection;
                @Direction.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnDirection;
                @Direction.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnDirection;
                @MousePosition.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnMousePosition;
                @Quit.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnQuit;
                @Quit.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnQuit;
                @Quit.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnQuit;
                @Test.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest;
                @Test.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest;
                @Test.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest;
                @Test1.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest1;
                @Test1.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest1;
                @Test1.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest1;
                @Test2.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest2;
                @Test2.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest2;
                @Test2.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest2;
                @Test3.started -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest3;
                @Test3.performed -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest3;
                @Test3.canceled -= m_Wrapper.m_NormalInputsActionsCallbackInterface.OnTest3;
            }
            m_Wrapper.m_NormalInputsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Direction.started += instance.OnDirection;
                @Direction.performed += instance.OnDirection;
                @Direction.canceled += instance.OnDirection;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Quit.started += instance.OnQuit;
                @Quit.performed += instance.OnQuit;
                @Quit.canceled += instance.OnQuit;
                @Test.started += instance.OnTest;
                @Test.performed += instance.OnTest;
                @Test.canceled += instance.OnTest;
                @Test1.started += instance.OnTest1;
                @Test1.performed += instance.OnTest1;
                @Test1.canceled += instance.OnTest1;
                @Test2.started += instance.OnTest2;
                @Test2.performed += instance.OnTest2;
                @Test2.canceled += instance.OnTest2;
                @Test3.started += instance.OnTest3;
                @Test3.performed += instance.OnTest3;
                @Test3.canceled += instance.OnTest3;
            }
        }
    }
    public NormalInputsActions @NormalInputs => new NormalInputsActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Return;
    private readonly InputAction m_UI_Move;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Click;
    public struct UIActions
    {
        private @PlayerInputs m_Wrapper;
        public UIActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Return => m_Wrapper.m_UI_Return;
        public InputAction @Move => m_Wrapper.m_UI_Move;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Return.started -= m_Wrapper.m_UIActionsCallbackInterface.OnReturn;
                @Return.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnReturn;
                @Return.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnReturn;
                @Move.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Return.started += instance.OnReturn;
                @Return.performed += instance.OnReturn;
                @Return.canceled += instance.OnReturn;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface INormalInputsActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnDirection(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnQuit(InputAction.CallbackContext context);
        void OnTest(InputAction.CallbackContext context);
        void OnTest1(InputAction.CallbackContext context);
        void OnTest2(InputAction.CallbackContext context);
        void OnTest3(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnReturn(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
    }
}
