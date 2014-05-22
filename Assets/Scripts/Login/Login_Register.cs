#if UNITY_ANDROID
/#define IAB




using System.Text;

#elif UNITY_IPHONE
//#define IAP
#endif
using System.Text;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Login_Register : MonoBehaviour
{
		public GUIStyle guiStyle;
		public GUISkin guiSkin;
		public Rect rectanguloVentana; //windowRect
		private int id_PaginaCargada = 0; //m_iCurrentPageAccount
		private String  str_nombreUsuario = ""; //m_strUser
		private String str_password = ""; //m_strPassword
		private String str_passwordRepetido = ""; //m_strPasswordRetype
		protected bool b_passwordValido = false;//m_bPasswordCheckedOK
		protected bool b_mostrarContraseña = false;//m_bShowPassword
		private PasswordScore pswScre_dificultadPassword;//m_PasswordScore
		protected bool b_passwordCorrecto = false;//m_bPasswordMatch
		private String str_email = "";//m_strEmail
		private bool recordarContraseña = false;

	private int numI_ancho;
	private int numI_alto;

		void OnGUI ()
		{
				rectanguloVentana = new Rect (10, 10, Screen.width - 20, Screen.height - 20);
				rectanguloVentana = GUILayout.Window (1, rectanguloVentana, CrearVentanaCliente, "", guiSkin.window, GUILayout.ExpandWidth (true));//expandwith permite o no la expansion de la ventana
		}
		//DoClientWindow
		void CrearVentanaCliente (int windowID)
		{
				String strPageNames = "Iniciar Sesion,Crear Cuenta";
				String[] strPageArray = strPageNames.Split ("," [0]);
				//			int iCurrentPageAccount;
				id_PaginaCargada = GUILayout.SelectionGrid (id_PaginaCargada, strPageArray, strPageArray.Length, guiSkin.button);
				//		if (m_iCurrentPageAccount != iCurrentPageAccount) {
				//				m_iCurrentPageAccount = iCurrentPageAccount;
				//				m_strPasswordRetype = "";
				//		}	
				switch (id_PaginaCargada) {
				case 0:
						DrawGUIAccountLogin ();
						break;
				case 1:
						DrawGUIAccountCreate ();
						break;
				}
				GUILayout.Label ("Recordar Contraseña", guiSkin.label, GUILayout.ExpandWidth (false));
		}

		protected void DrawGUIAccountLogin ()
		{
				GUILayout.BeginVertical ();
				GUILayout.Label ("Usuario", guiSkin.label, GUILayout.ExpandWidth (false));
				str_nombreUsuario = GUILayout.TextField (str_nombreUsuario, 30, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				GUILayout.Label ("Contraseña", guiSkin.label, GUILayout.ExpandWidth (false));		
				str_password = GUILayout.PasswordField (str_password, "*" [0], 16, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				
				if (GUILayout.Button ("Entrar", guiSkin.button, GUILayout.Width (200))) {	
					
				}
				GUILayout.EndVertical ();
		}

		protected void DrawGUIAccountCreate ()
		{
				GUILayout.BeginVertical ();
				GUILayout.Label ("Usuario", guiSkin.label, GUILayout.ExpandWidth (false));

				str_nombreUsuario = GUILayout.TextField (str_nombreUsuario, 30, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				GUILayout.Label ("Contraseña (Min. 6)", guiSkin.label, GUILayout.ExpandWidth (false));
				String strPassword;
				if (b_mostrarContraseña)
						strPassword = GUILayout.TextField (str_password, 16, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				else
						strPassword = GUILayout.PasswordField (str_password, "*" [0], 16, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				
				if (strPassword != str_password) {			
						str_password = strPassword;
						//CheckPassword(strPassword);
						pswScre_dificultadPassword = PasswordAdvisor.CheckStrength (strPassword);
						b_passwordValido = pswScre_dificultadPassword >= PasswordScore.Media;			
						b_passwordCorrecto = str_password == str_passwordRepetido;
				}
				if (str_password.Length > 5) {
						GUILayout.Label ("Complejidad:" + pswScre_dificultadPassword.ToString (), guiSkin.label, GUILayout.ExpandWidth (false));
				}	
				GUILayout.Label ("Repetir Contraseña", guiSkin.label, GUILayout.ExpandWidth (false));
				
				if (b_mostrarContraseña)
						str_passwordRepetido = GUILayout.TextField (str_passwordRepetido, 16, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));
				else
						str_passwordRepetido = GUILayout.PasswordField (str_passwordRepetido, "*" [0], 16, guiSkin.textField, GUILayout.Width (200), GUILayout.Height (25));

				b_passwordCorrecto = str_password == str_passwordRepetido;	
				if (!b_passwordCorrecto && str_passwordRepetido.Length > 5)
						//TODO: Funcion para aparicion que solo aparezca "contraseñas diferentes" cuando se compruebe.
						GUILayout.Label ("Contraseñas diferentes", guiSkin.label, GUILayout.ExpandWidth (false));		


				bool boolenabled = false;
				b_mostrarContraseña = GUILayout.Toggle (b_mostrarContraseña, "Mostrar contraseña", guiSkin.toggle);
				GUILayout.Label ("E-mail", guiSkin.label, GUILayout.ExpandWidth (false));
				str_email = GUILayout.TextField (str_email, 64, guiSkin.textField, GUILayout.Width (300), GUILayout.Height (25));

				if (b_passwordValido && b_passwordCorrecto && str_password.Length > 5 && str_nombreUsuario.Length > 2 && PasswordAdvisor.IsEmail (m_strEmail)) {
						GUI.enabled = true; 
				} else {
						GUI.enabled = false; 
				}
				if (GUILayout.Button ("Crear cuenta", guiSkin.button, GUILayout.Width (200))) {

				}
				GUI.enabled = true; 
				GUILayout.EndVertical ();
		}

		private string Md5Sum (string input)
		{
				System.Security.Cryptography.MD5 
				md5 = System.Security.Cryptography.MD5.Create ();
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input);
				byte[] hash = md5.ComputeHash (inputBytes);
				StringBuilder sb = new StringBuilder ();
				for (int i = 0; i < hash.Length; i++) {
						sb.Append (hash [i].ToString ("X2"));
				}
				return sb.ToString ();
		}

		public enum PasswordScore
		{
				Vacia = 0,
				Facilisima = 1,
				Facil = 2,
				Media = 3,
				Dificil = 4,
				Dificilisima = 5
		}

		public class PasswordAdvisor
		{
				public enum PasswordRules
				{
						/// <summary>
						/// Password must contain a digit
						/// </summary>
						Digit = 1,
						/// <summary>
						/// Password must contain an uppercase letter
						/// </summary>
						UpperCase = 2,
						/// <summary>
						/// Password must contain a lowercase letter
						/// </summary>
						LowerCase = 4,
						/// <summary>
						/// Password must have both upper and lower case letters
						/// </summary>
						MixedCase = 6,
						/// <summary>
						/// Password must include a non-alphanumeric character
						/// </summary>
						SpecialChar = 8,
						/// <summary>
						/// All rules should be checked
						/// </summary>
						All = 15,
						/// <summary>
						/// No rules should be checked
						/// </summary>
						None = 0
				}

				public const string MatchEmailPattern = 
			    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"		
						+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."			
						+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"			
						+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
		
			
				/// <summary>
				/// Checks whether the given Email-Parameter is a valid E-Mail address.
				/// </summary>
				/// <param name="email">Parameter-string that contains an E-Mail address.</param>
				/// <returns>True, wenn Parameter-string is not null and contains a valid E-Mail address;
				/// otherwise false.</returns>
		
				public static bool IsEmail (string email)
				{
						if (email != null)
								return Regex.IsMatch (email, MatchEmailPattern);
						else
								return false;
				}

				/// <returns>
				/// <c>true</c> if this instance is password valid the specified password rules ruleOutList; otherwise, <c>false</c>.
				/// </returns>
				/// <param name='password'>
				/// If set to <c>true</c> password.
				/// </param>
				/// <param name='rules'>
				/// If set to <c>true</c> rules.
				/// </param>
				/// <param name='ruleOutList'>
				/// If set to <c>true</c> rule out list.
				/// </param>
				public static bool IsPasswordValid (string password,
		                                   PasswordRules rules,
		                                   params string[] ruleOutList)
				{
						bool result = true;
						const string lower = "abcdefghijklmnopqrstuvwxyz";
						const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						const string digits = "0123456789";
						string allChars = lower + upper + digits;
						//Check Lowercase if rule is enforced
						if (Convert.ToBoolean (rules & PasswordRules.LowerCase)) {
								result &= (password.IndexOfAny (lower.ToCharArray ()) >= 0);
						}
						//Check Uppercase if rule is enforced
						if (Convert.ToBoolean (rules & PasswordRules.UpperCase)) {
								result &= (password.IndexOfAny (upper.ToCharArray ()) >= 0);
						}
						//Check to for a digit in password if digit is required
						if (Convert.ToBoolean (rules & PasswordRules.Digit)) {
								result &= (password.IndexOfAny (digits.ToCharArray ()) >= 0);
						}
						//Check to make sure special character is included if required
						if (Convert.ToBoolean (rules & PasswordRules.SpecialChar)) {
								result &= (password.Trim (allChars.ToCharArray ()).Length > 0);
						}
						if (ruleOutList != null) {
								for (int i = 0; i < ruleOutList.Length; i++)
										result &= (password != ruleOutList [i]);
						}
						return result;
				}

				public static PasswordScore CheckStrength (string password)
				{
						int score = 1;
			
						if (password.Length < 1)
								return PasswordScore.Vacia;
						if (password.Length < 4)
								return PasswordScore.Facilisima;
			
						if (password.Length >= 6)
								score++;
						if (password.Length >= 12)
								score++;
						if (Regex.IsMatch (password, @"\d+"))
								score++;
						if (Regex.IsMatch (password, @"[a-z]") && Regex.IsMatch (password, @"[A-Z]"))
								score++;
						if (Regex.IsMatch (password, @"[!@#\$%\^&\*\?_~\-\(\);\.\+:]+"))
								score++;
			
						return (PasswordScore)score;
				}
		}
}

