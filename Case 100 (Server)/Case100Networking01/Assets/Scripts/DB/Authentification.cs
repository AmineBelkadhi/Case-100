using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using UnityEngine.SceneManagement;

public class Authentification : MonoBehaviour
{
    private Mongo db;

    public TMP_InputField emailText;
    public TMP_InputField passwordText;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private TextMeshProUGUI errorText;

    private void Start()
    {
        ConnectToDB();

        //db.InsertAccount("username", "password", "email");
    }

    private void ConnectToDB()
    {
        db = new Mongo();
        db.Init();
    }

    public bool AttemptLogin()
    {
        if (emailText == null || passwordText == null)
        {
            Debug.Log("Assign Text Fields in the inspector");
            return false;
        }

        if (emailText.text == string.Empty)
        {
            Debug.Log("Mail field is empty");
            errorText.text = "You have to fill both the email and the password";
            return false;
        }

        if (db.LoginAccount(emailText.text, passwordText.text, 0, Utility.GenerateRandom(5)) != null)
        {
            loginPanel.SetActive(false);
            mainMenuPanel.SetActive(true);

            emailText.text = "";
            passwordText.text = "";
            return true;
        }
        else
        {
            Debug.Log("not found");
            errorText.text = "Email or password not correct";
        }

        return false;
    }

    public void Login()
    {
        AttemptLogin();
    }

    public void Logout()
    {
        loginPanel.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}