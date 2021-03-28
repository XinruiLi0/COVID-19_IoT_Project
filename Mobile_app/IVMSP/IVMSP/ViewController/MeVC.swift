//
//  MeVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2021/1/2.
//

import UIKit

class MeVC: UIViewController {
    
    @IBOutlet var userName: UILabel!
    @IBOutlet var userEmailLabel: UILabel!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        userName.text = UserDefaults.standard.string(forKey: "UserName")!
        userEmailLabel.text = UserDefaults.standard.string(forKey: "UserEmail")!
        
        
        
    }
    
    
    @IBAction func onLogOut(_ sender: Any) {
        
        guard let window = UIApplication.shared.windows.first else {
            return
        }
        
        
        // create the alert
        let alert = UIAlertController(title: "Confirm", message: "Logout will delete all data in this app", preferredStyle: UIAlertController.Style.alert)
        
        // add the actions (buttons)
        alert.addAction(UIAlertAction(title: "Cancel", style: UIAlertAction.Style.cancel, handler: nil))
        alert.addAction(UIAlertAction(title: "Log Out", style: UIAlertAction.Style.destructive, handler: { action in
            
            let storyboard = UIStoryboard(name: "Main", bundle: nil)
            let viewController = storyboard.instantiateViewController(withIdentifier: "Login") as! LoginVC

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
                                    if let appDomain = Bundle.main.bundleIdentifier {
                                        UserDefaults.standard.removePersistentDomain(forName: appDomain)
                                    }

                                    //            UserDefaults.standard.setValue(false, forKey: "loggedIn")
                                })

        }))
        
        // show the alert
        self.present(alert, animated: true, completion: nil)
        
        
        
        
        
        


        
        
    }
}
