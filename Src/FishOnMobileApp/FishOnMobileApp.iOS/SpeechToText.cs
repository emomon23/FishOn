using System;
using AVFoundation;
using Foundation;
using Speech;
using VoiceRecognitionSystem.iOS;

[assembly: Xamarin.Forms.Dependency (typeof (SpeechToTextImplementation))]
namespace VoiceRecognitionSystem.iOS
{
	public class SpeechToTextImplementation : ISpeechToText
	{
		public event EventHandler<EventArgsVoiceRecognition> textChanged;
		#region Private Variables
		private AVAudioEngine AudioEngine = new AVAudioEngine ();
		private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer ();
		private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest ();
		private SFSpeechRecognitionTask RecognitionTask;
		#endregion
		public SpeechToTextImplementation ()
		{
		}
		public void InitializeProperties ()
		{
			AudioEngine = new AVAudioEngine ();
			SpeechRecognizer = new SFSpeechRecognizer ();
			LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest ();
		}

		public void Start ()
		{
			AskPermission ();
		}
		public void Stop ()
		{
			CancelRecording ();
		}
		void AskPermission ()
		{
			// Request user authorization
			SFSpeechRecognizer.RequestAuthorization ((SFSpeechRecognizerAuthorizationStatus status) => {
				// Take action based on status
				switch (status) {
				case SFSpeechRecognizerAuthorizationStatus.Authorized:
					InitializeProperties ();
					StartRecordingSession ();
					break;
				case SFSpeechRecognizerAuthorizationStatus.Denied:
					// User has declined speech recognition

					break;
				case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
					// Waiting on approval

					break;
				case SFSpeechRecognizerAuthorizationStatus.Restricted:
					// The device is not permitted

					break;
				}
			});
		}

		public void StartRecordingSession ()
		{
			// Start recording
			AudioEngine.InputNode.InstallTapOnBus (
	    bus: 0,
	    bufferSize: 1024,
	    format: AudioEngine.InputNode.GetBusOutputFormat (0),
	    tapBlock: (buffer, when) => LiveSpeechRequest?.Append (buffer));
			AudioEngine.Prepare ();
			NSError error;
			AudioEngine.StartAndReturnError (out error);

			// Did recording start?
			if (error != null) {
				// Handle error and retur
				return;
			}

			try {
				CheckAndStartReconition ();
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}

		}
		public void CheckAndStartReconition ()
		{
			if (RecognitionTask?.State == SFSpeechRecognitionTaskState.Running) {
				CancelRecording ();
			}
			StartVoiceRecognition ();
		}
		public void StartVoiceRecognition ()
		{
			try {

				RecognitionTask = SpeechRecognizer.GetRecognitionTask (LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) => {
					if (result == null) {
						CancelRecording ();
						return;
					}
					// Was there an error?
					if (err != null) {
						CancelRecording ();
						return;
					}
					//	 Is this the final translation?
					if (result != null && result.BestTranscription != null && result.BestTranscription.FormattedString != null) {
						Console.WriteLine ("You said \"{0}\".", result.BestTranscription.FormattedString);
						TextChanged (result.BestTranscription.FormattedString);
					}
					if (result.Final) {
						TextChanged (result.BestTranscription.FormattedString, true);
						CancelRecording ();
						return;
					}
				});
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}
		public void StopRecording ()
		{
			try {
				AudioEngine?.Stop ();
				LiveSpeechRequest?.EndAudio ();
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public void CancelRecording ()
		{
			try {
				AudioEngine?.Stop ();
				RecognitionTask?.Cancel ();
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}
		public void TextChanged (string text, bool isFinal = false)
		{
			textChanged?.Invoke (this, new EventArgsVoiceRecognition (text, isFinal));
		}


	}
}
