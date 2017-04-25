using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace FishOn.VoiceRecognitionSystem
{
    //This interface is used for IOS Speech to text.
    //(Android has a cludgy hook, and doesn't use this interface)
	public interface ISpeechToText
	{
		void Start ();
		void Stop ();
		event EventHandler<EventArgsVoiceRecognition> textChanged;
	}

    public delegate void VoiceConvertedToTextDelegate(string value);
    public delegate void NoMicrophoneIsAvailableDelegate();

    //The way we hook into android voice to text is very different than how we hook into IOS.
    //The classes and interface in this file, help to encapsulate all of that logic and create a simple way 
    //to hook into the native voice to text
    public class VoiceToText
    {
        private List<VoiceToTextBinder> binders = new List<VoiceToTextBinder>();
        private StackLayout layout;
        private NoMicrophoneIsAvailableDelegate NoMicrophoneCallback { get; set; }

        public VoiceToText(StackLayout layout)
        {
            this.layout = layout;
        }

        //Bind a view (label, layout, image, etc), along with an entry control to speech to voice
        public VoiceToTextBinder BindToNativeSpeechRecognistion(Entry entry, View clickableView = null, VoiceConvertedToTextDelegate voiceConvertedToTextCallback = null) 
        {
            VoiceToTextBinder binder = new VoiceToTextBinder(layout);
            if (binders.Count == 0)
            {
                binder.NoMicrophoneIsAvailable_Notification = NoMicrophoneCallback;
            }

            binder.VoiceConvertedToText_Event = voiceConvertedToTextCallback;
            binder.BindToNativeSpeechRecognistion(entry, clickableView);
            
            binders.Add(binder);
            return binder;
        }

        public VoiceToTextBinder BindToNativeSpeechRecognistion(Label entry, View clickableView = null, VoiceConvertedToTextDelegate voiceConvertedToTextCallback = null)
        {
            VoiceToTextBinder binder = new VoiceToTextBinder(layout);
            if (binders.Count == 0)
            {
                binder.NoMicrophoneIsAvailable_Notification = NoMicrophoneCallback;
            }

            binder.VoiceConvertedToText_Event = voiceConvertedToTextCallback;
            binder.BindToNativeSpeechRecognistion(entry, clickableView);

            binders.Add(binder);
            return binder;
        }
    }

    public class VoiceToTextBinder { 

       
        private bool amTheActiveVoiceToTextListner = false;
        private StackLayout layout;

        //For android, we need a hook into a view, we will use a custom 'VoiceButton' (see definition below)
        private VoiceButton androidVoiceButton;

        //For ios we will implement an interface
        private ISpeechToText iOSSpeechRecognitionInstnace;

        //These controls, if provided, will be updated to display the text that was recorded
        private Entry speechToTextEntry;
        private Label lblSpeecToTextEntry;

        private bool isIOSRecording = false;

        private ICommand recordCommand;
       
        //Constructor
        public VoiceToTextBinder(StackLayout layout)
        {
            this.layout = layout;
        }

        public VoiceConvertedToTextDelegate VoiceConvertedToText_Event { get; set; }
        public NoMicrophoneIsAvailableDelegate NoMicrophoneIsAvailable_Notification { get; set; }

        //Bind a view (label, layout, image, etc), along with an entry control to speech to voice
        public void BindToNativeSpeechRecognistion(Entry entry, View clickableView = null)
        {
            this.speechToTextEntry = entry;
            SetupSpeechToText(clickableView);
        }

        public void BindToNativeSpeechRecognistion(Label entry, View clickableView = null)
        {
            this.lblSpeecToTextEntry = entry;
            SetupSpeechToText(clickableView);
        }

        public ICommand RecordButtonCommand
        {
            get
            {
                if (recordCommand == null)
                {
                    //When 'clickable' is clicked, call this command logic
                    recordCommand = new Command(() =>
                    {
                        SetupNativeHooks();
                       
                        if (Device.OS == TargetPlatform.Android)
                        {
                            amTheActiveVoiceToTextListner = true;
                            androidVoiceButton.LaunchNavtiveVoiceRecognitionIntent();
                        }
                        else if (Device.OS == TargetPlatform.iOS)
                        {
                            if (isIOSRecording)
                            {
                                iOSSpeechRecognitionInstnace.Stop();
                                amTheActiveVoiceToTextListner = false;
                            }
                            else
                            {
                                amTheActiveVoiceToTextListner = true;
                                iOSSpeechRecognitionInstnace.Start();
                            }

                            isIOSRecording = !isIOSRecording;
                        }

                        TeardownNativeHooks();
                    });

                   
                }

                return recordCommand;
            }
        }

        private void SetupSpeechToText(View clickableControl) { 
            //Assign the command defined above to the clickableControl
            if (clickableControl != null)
            {
                if (clickableControl.GetType().Name == "Button")
                {
                    Button btn = (Button) clickableControl;
                    btn.Command = this.RecordButtonCommand;
                }
                else
                {
                    TapGestureRecognizer tap = new TapGestureRecognizer()
                    {
                        Command = this.RecordButtonCommand
                    };

                    clickableControl.GestureRecognizers.Add(tap);
                }
            }
        }

        private void SetupNativeHooks()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                SetupAndroidVoiceButton();
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                SetUpIOSSpeechRecognizer();
            }
        }

        private void TeardownNativeHooks()
        {
            layout.Children.Remove(androidVoiceButton);
        }

        private void SetupAndroidVoiceButton()
        {
            //the androidVoiceButton is an instance of VoiceButton (and it inherits from Button)
            //but we might not want to use an actual button, we might want to use a label,
            //or an image, or any other tap gesture away view as the clickable control to launch speech to text.

            //Create a VoiceButton, set it's height and width to 0 and color to tranparent
            //then add it to the layout so that we can hook into it when our clickableView is clicked (or tapped).
            androidVoiceButton = new VoiceButton()
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 0,
                WidthRequest = 0
            };

           
            layout.Children.Add(androidVoiceButton);

            androidVoiceButton.OnTextChanged += (s) => {
                if (amTheActiveVoiceToTextListner)
                {
                    if (s == androidVoiceButton.NO_MICROPHONE_AVAILABLE)
                    {
                        NoMicrophoneIsAvailable_Notification?.Invoke();
                    }

                    if (speechToTextEntry != null)
                    {
                        speechToTextEntry.Text = s;
                    }

                    if (lblSpeecToTextEntry != null)
                    {
                        lblSpeecToTextEntry.Text = s;
                    }

                    VoiceConvertedToText_Event?.Invoke(s);
                    amTheActiveVoiceToTextListner = false;
                }
            };
        }

        private void SetUpIOSSpeechRecognizer()
        {
            iOSSpeechRecognitionInstnace = DependencyService.Get<ISpeechToText>();
            iOSSpeechRecognitionInstnace.textChanged += OnIOS_VoiceToTextChange;
        }

        //When speech is converted to text on IOS, this method will get called.
        private void OnIOS_VoiceToTextChange(object sender, EventArgsVoiceRecognition e)
        {
            if (speechToTextEntry != null)
            {
                speechToTextEntry.Text = e.Text;
            }

            if (lblSpeecToTextEntry != null)
            {
                lblSpeecToTextEntry.Text = e.Text;
            }

            if (e.IsFinal)
            {
                VoiceConvertedToText_Event(e.Text);
            }
        }
    }

    //Used by the ISpeechToText for iOS speech to text
    public class EventArgsVoiceRecognition : EventArgs
    {
        public EventArgsVoiceRecognition(string text, bool isFinal)
        {
            this.Text = text;
            this.IsFinal = isFinal;
        }
        public string Text { get; set; }
        public bool IsFinal { get; set; }

    }

    //The SpeechToText.cs file found in the .Droid project utialized this class
    //to hook into the voice to text
    public class VoiceButton : Button
    {
        public delegate void LaunchNavtiveVoiceRecognitionDelegate();

        public Action<string> OnTextChanged { get; set; }
        public LaunchNavtiveVoiceRecognitionDelegate LaunchNavtiveVoiceRecognitionIntent { get; set; }

        public string NO_MICROPHONE_AVAILABLE
        {
            get { return "~NO_MICROW_PHONE_IS_AVAILABLE~"; }
        }
    }
}

