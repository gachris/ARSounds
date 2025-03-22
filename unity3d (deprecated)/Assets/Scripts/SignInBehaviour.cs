using Assets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignInBehaviour : MonoBehaviour
{
    private Token _token;
    private bool _signedIn;
    private bool _signinCancelled;
    private bool _authOperationInProgress;
    private const string SignInText = "Sign in";
    private const string SignOutText = "Sign out";
    public GameObject SignInButtonText;
    public Button SignInButton;
    private UnityAuthClient _authClient;
#if !UNITY_STANDALONE
    private bool _replyReceived;
    private bool _watchForReply;
    private DateTime _watchForReplyStartTime;
    private const double MaxSecondsToWaitForAuthReply = 3;
#endif

    [Header("UI")]
    public LoadingController _loadingController;

    protected virtual async void Start()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        Debug.Log("SignInBehavior::Start");

        _authClient = new UnityAuthClient();

        _token = await _authClient.GetToken();
        _signedIn = _token != null;

        EnableSignInButton(true);
    }

    public async void OnSignInClicked()
    {
        EnableSignInButton(false);

        _signinCancelled = false;
        _authOperationInProgress = true;

#if !UNITY_STANDALONE
        this._replyReceived = false;
        this._watchForReply = false;
#endif

        if (_signedIn)
        {
            await SignOut();
        }
        else
        {
            await SignIn();
        }

        EnableSignInButton(true);
    }

    private async Task SignIn()
    {
        Debug.Log("SignInBehavior::Signing in...");
        _loadingController.Show("Login...");

        _token = await _authClient.LoginAsync();
        _signedIn = _token != null;
        _authOperationInProgress = false;

#if !UNITY_STANDALONE
        this._watchForReply = false;
#endif

        if (_signedIn)
        {
            ToastMessage.Show("Sign in successful.", ToastMessage.Position.bottom, ToastMessage.Time.twoSecond);
            Debug.Log("SignInBehavior::Sign-in successful.");
        }
        else if (_signinCancelled)
        {
            ToastMessage.Show("Sign-in was cancelled by the user.", ToastMessage.Position.bottom, ToastMessage.Time.twoSecond);
            Debug.Log("SignInBehavior::Sign-in was cancelled by the user.");
            //this.StatusText.GetComponent<Text>().text = "Sign-in cancelled.";
        }
        else
        {
            MessageBox.Show("Test", "Failed to perform sign-in.", true, null);
            Debug.Log("SignInBehavior::Failed to perform sign-in.");
        }

        _loadingController.Hide();
    }

    private async Task SignOut()
    {
        Debug.Log("SignInBehavior::Signing out...");

        if (_token != null)
        {
            _signedIn = !await _authClient.LogoutAsync(_token.IdentityToken);
        }

        _authOperationInProgress = false;
#if !UNITY_STANDALONE
        this._watchForReply = false;
#endif

        if (!_signedIn)
        {
            MessageBox.Show("Aug", "Sign-out successful.", true, null);
            //this.StatusText.GetComponent<Text>().text = "";
        }
        else if (_signinCancelled)
        {
            Debug.Log("SignInBehavior::Sign-out was cancelled by the user.");
            MessageBox.Show("SignInBehavior", "Sign-out cancelled.", true, null);
        }
        else
        {
            Debug.Log("SignInBehavior::Failed to perform sign-out.");
            //this.StatusText.GetComponent<Text>().text = "An error occurred during sign out.  Please ensure you have Internet access.";
        }
    }

    private void EnableSignInButton(bool enabled)
    {
        var text = _signedIn ? SignInBehaviour.SignOutText : SignInBehaviour.SignInText;
        SignInButtonText.GetComponent<Text>().text = text;
        SignInButton.GetComponent<Button>().interactable = enabled;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("SignInBehavior::OnApplicationPause: " + pauseStatus);
        var resumed = !pauseStatus;
        if (resumed)
        {
            Debug.Log("SignInBehavior::App was resumed.");
            if (_authOperationInProgress)
            {
#if !UNITY_STANDALONE
                if (!_replyReceived)
                {
                    Debug.Log("SignInBehavior::Watching for auth reply.");
                    this._watchForReply = true;
                    this._watchForReplyStartTime = DateTime.Now;
                }
#endif
            }
            else
            {
                // App has been resumed, but we are not signing in, e.g. user has closed the sign-out browser.
                EnableSignInButton(true);
            }
        }
    }

    private void Update()
    {
#if !UNITY_STANDALONE
        if (this._watchForReply && DateTime.Now - this._watchForReplyStartTime > TimeSpan.FromSeconds(SignInBehaviour.MaxSecondsToWaitForAuthReply))
        {
            Debug.Log("SignInBehavior::No auth reply received, assuming the user cancelled or was unable to complete the sign-in.");
            this._watchForReply = false;
            this._signinCancelled = true;
            this._authClient.OnAuthReply(null);
        }
#endif
    }

#if !UNITY_STANDALONE
    public void OnAuthReply(object value)
    {
        if (!this._signinCancelled)
        {
            this._watchForReply = false;
            this._replyReceived = true;
            Debug.Log("SignInBehavior::OnAuthReply: " + value);
            this._authClient.OnAuthReply(value as string);
        }
    }
#endif
}
