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

    @IBOutlet var loginStackView: UIStackView!
    
    @IBAction func onLogin(_ sender: UIButton) {
        
        ////// get auth
        ////// attempt in
        
        print("loginnnn")
    }
    
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
