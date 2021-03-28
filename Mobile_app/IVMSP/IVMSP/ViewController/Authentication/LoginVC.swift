//
//  LoginVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2020/11/12.
//

import UIKit

class LoginVC: UIViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.hideKeyboardWhenTappedAround()
    }
    
    @IBOutlet var emailField: UITextField!
    
    @IBOutlet var passwordField: UITextField!
    
    @IBOutlet var loginStackView: UIStackView!
    
    
    
    @IBAction func onLogin(_ sender: UIButton) {
        var urlComps =  URLComponents(string: "http://ivmsp.us-east-1.elasticbeanstalk.com/Login/userLogin")!
        
        
        let queryItems = [URLQueryItem(name: "userEmail", value: emailField.text), URLQueryItem(name: "userPassword", value: passwordField.text), URLQueryItem(name: "userRole", value: "1")]
        urlComps.queryItems = queryItems
        
        print (urlComps.url!)
        
        var request = URLRequest(url: urlComps.url!)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any]
                    
                    let result = json?["result"] as! String
                    let message = json?["message"] as! String

                    if (result  == "error") {
                        DispatchQueue.main.async {
                            let alert = UIAlertController(title: "Log In Failed", message: message, preferredStyle: .alert)
                            alert.addAction(UIAlertAction(title: "OK", style: .default, handler: nil ))
                            self.present(alert, animated: true, completion: nil)
                        }
                    } else {
                        DispatchQueue.main.async {
                            showTabView(userEmail: String(self.emailField.text!), userName: message)
                        }
                    }
                    
                } catch {
                    print(error)
                }
            }
            
        }.resume()
        
        
        
        
        //
        //        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        //        let viewController = storyboard.instantiateViewController(withIdentifier: "TabBar") as! UITabBarController
        //        UIApplication.shared.windows.first?.rootViewController = viewController
        //        UIApplication.shared.windows.first?.makeKeyAndVisible()
    }
    
}


func showTabView(userEmail: String, userName: String) {
    
    guard let window = UIApplication.shared.windows.first else {
        return
    }
    
    let storyboard = UIStoryboard(name: "Main", bundle: nil)
    let viewController = storyboard.instantiateViewController(withIdentifier: "TabBar") as! UITabBarController
    
    // Set the new rootViewController of the window.
    // Calling "UIView.transition" below will animate the swap.
    window.rootViewController = viewController
    
    // A mask of options indicating how you want to perform the animations.
    let options: UIView.AnimationOptions = .transitionCrossDissolve
    
    // The duration of the transition animation, measured in seconds.
    let duration: TimeInterval = 0.3
    
    // Creates a transition animation.
    // Though `animations` is optional, the documentation tells us that it must not be nil. ¯\_(ツ)_/¯
    UIView.transition(with: window, duration: duration, options: options, animations: {}, completion:
                        { completed in
                            // maybe do something on completion here
                            UserDefaults.standard.setValue(true, forKey: "loggedIn")
                            
                            UserDefaults.standard.setValue(userEmail, forKey: "UserEmail")
                            UserDefaults.standard.setValue(userName, forKey: "UserName")
                        })
}


extension UIViewController {
    func hideKeyboardWhenTappedAround() {
        let tap = UITapGestureRecognizer(target: self, action: #selector(UIViewController.dismissKeyboard))
        tap.cancelsTouchesInView = false
        view.addGestureRecognizer(tap)
    }
    
    @objc func dismissKeyboard() {
        view.endEditing(true)
    }
}
