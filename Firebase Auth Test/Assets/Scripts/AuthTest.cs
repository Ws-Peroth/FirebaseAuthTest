using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth; // 계정인증기능 사용

public class AuthTest : MonoBehaviour
{
    [SerializeField] Text errorTextUI;
    public string email = "";
    public string password = "";
    string errorText = "";
    public bool isLogin = false;
    private FirebaseAuth auth; // 인증 객체 불러오기
    DatabaseReference Reference;
    void Start()
    {
        Reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Uid").Reference;
        auth = FirebaseAuth.DefaultInstance; // 인증 객체 초기화
        print("E-Mail : tester@gmail.com, \nPw = password");

    }

    private void Update()
    {
        errorTextUI.text = errorText;
    }

    public void LoginButtonDown()
    {
        Login(email, password);
    }

    public void SignButtonDown()
    {
        Join(email, password);
    }


    void Join(string email, string password)
    {
        isLogin = false;
        // 이메일과 비밀번호로 가입하는 함수
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
             task =>
             {
                 if (!task.IsCanceled && !task.IsFaulted)
                 {
                     errorText = email + " 로 회원가입 하셨습니다.";
                 }
                 else
                 {
                     Debug.Log("회원가입에 실패하셨습니다.");

                     if (task.IsCanceled)
                     {
                         errorText = "Create User With Email And Password Async was canceled.";
                         return;
                     }
                     if (task.IsFaulted)
                     {
                         errorText = task.Exception.ToString();
                         PrintErrorCode();
                         return;
                     }
                     
                 }
             }
         );
    }
    
    void PrintErrorCode()
    {
        print(errorText);

        string result = errorText.Split(
               new string[] { "Firebase.FirebaseException:" },
               System.StringSplitOptions.None)[1];

        errorText = result.Split('\n')[0];

        print(result);
        errorTextUI.text = result;
    }

    void Login(string email, string password)
    {
        // 이메일과 비밀번호로 가입하는 함수
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task => {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    errorText = email + " 로 로그인 하셨습니다.";
                    isLogin = true;
                }
                else
                {
                    errorText = "로그인에 실패하셨습니다.";
                    isLogin = false;
                }
            }
        );
    }

    public void Logout()
    {
        errorText = "로그아웃 완료";
        auth.SignOut();
        isLogin = false;
    }

    public void MakeAccount()
    {
        if (isLogin)
        {
            print(auth.CurrentUser.UserId);
        }
    }

}
