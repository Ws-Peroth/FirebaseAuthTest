using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth; // 계정인증기능 사용

public class AuthTest : MonoBehaviour
{
    public Text errorText;
    private FirebaseAuth auth; // 인증 객체 불러오기

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance; // 인증 객체 초기화
        Join("tester@gmail.com", "password"); // 해당 이메일,비밀번호로 가입하기
        // Login("tester@gmail.com", "password");
        print("E-Mail : tester@gmail.com, \nPw = password");
    }

    void Join(string email, string password)
    {
        // 이메일과 비밀번호로 가입하는 함수
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
             task =>
             {
                 if (!task.IsCanceled && !task.IsFaulted)
                 {
                     Debug.Log(email + " 로 회원가입 하셨습니다.");
                     errorText.text = email + " 로 회원가입 하셨습니다.";
                 }
                 else
                 {
                     Debug.Log("회원가입에 실패하셨습니다.");
                     if (task.IsCanceled)
                     {
                         print("Create User With Email And Password Async was canceled.");
                         return;
                     }
                     if (task.IsFaulted)
                     {
                         string errorCode = task.Exception.ToString();
                         PrintErrorCode(errorCode);
                         return;
                     }
                     
                 }
             }
         );
    }
    
    void PrintErrorCode(string errorCode)
    {
        print(errorCode);

        string result = errorCode.Split(
               new string[] { "Firebase.FirebaseException:" },
               System.StringSplitOptions.None)[1];

        result = result.Split('\n')[0];

        print(result);
        errorText.text = result;
    }

    void Login(string email, string password)
    {
        // 이메일과 비밀번호로 가입하는 함수
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task => {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(email + " 로 로그인 하셨습니다.");
                }
                else
                {
                    Debug.Log("로그인에 실패하셨습니다.");
                }
            }
        );
    }

    public void SetData()
    {

    }
}
