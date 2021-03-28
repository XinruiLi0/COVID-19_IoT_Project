//
//  HomeVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2020/12/27.
//

import UIKit
import CoreNFC

class HomeVC: UIViewController, NFCTagReaderSessionDelegate {
    
    var userEmail = "1181536731@qq.com"
    var userPassword = "1181536731"
    var alerted = false
    var deviceID = "None"
    
    var checkedInFlag: Bool = false {
        didSet{
            if checkedInFlag == true {
                self.checkINOUTbutton.setTitle("Check Out", for: .normal)
            } else {
                self.checkINOUTbutton.setTitle("Check In", for: .normal)
            }
            
        }
    }
    @IBOutlet var backview: UIView!
    
    
    @IBOutlet  var textView: UITextView!
    
    @IBOutlet var checkINOUTbutton: UIButton!
    
    var nfcSession: NFCTagReaderSession?
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.hideKeyboardWhenTappedAround()
        
        
        self.textView.text = "Health Status:\n Good"
        DispatchQueue.global(qos: .background).async {
            while (!self.alerted) {
                sleep(1)
                self.selfCheck(userEmail: self.userEmail, userPassword: self.userPassword)
                
            }
            
            
        }
        
        
    }
    
    
    
    @IBAction func onCheckButton(_ sender: Any) {
        
        let actionSheet = UIAlertController(title: "Scan", message: nil, preferredStyle: .actionSheet)
        
        actionSheet.addAction(UIAlertAction(title: "NFC Tag", style: .default, handler:{
            action in
            // scan by NFC tag
            self.nfcSession = NFCTagReaderSession(pollingOption: .iso14443, delegate: self)
            self.nfcSession?.alertMessage = "Please Hold Near NFC Tag"
            self.nfcSession?.begin()
        }))
        
        actionSheet.addAction(UIAlertAction(title: "QR Code", style: .default, handler:{
            action in
            // scan by QR code
            self.performSegue(withIdentifier: "showQRscan", sender: sender)
        }))
        
        actionSheet.addAction(UIAlertAction(title: "Cancel", style: .cancel, handler: nil))
        
        self.present(actionSheet, animated: true, completion: nil)
        
    }
    
    
    
    func tagReaderSessionDidBecomeActive(_ session: NFCTagReaderSession) {
        print("Session Begun")
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didInvalidateWithError error: Error) {
        print(error)
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didDetect tags: [NFCTag]) {
        print("Detected NFC tag")
        
        if tags.count > 1 {
            session.alertMessage = "More than one tag detected"
            session.invalidate()
        }
        
        let tag = tags.first!
        session.connect(to: tag) {(error) in
            if nil != error{
                session.invalidate(errorMessage: "connection failed")
            }
            if case let .miFare(sTag) = tag {
                let UID = sTag.identifier.map{ String(format: "%.2hhx", $0)}.joined()
                print("UID:", UID)
                session.alertMessage = "Success"
                session.invalidate()
                DispatchQueue.main.async {
                    
                    self.deviceID = UID
                    
                    self.sendCheckRecord(deviceID: self.deviceID, visitorEmail: self.userEmail)
                }
            }
        }
    }
    
    
    func sendCheckRecord(deviceID: String, visitorEmail: String) {
        var urlComps = checkedInFlag ? URLComponents(string: "http://ivmsp.us-east-1.elasticbeanstalk.com/Home/leavingVisitorUpdate")! : URLComponents(string: "http://ivmsp.us-east-1.elasticbeanstalk.com/Home/visitorDetect")!
        
        
        let queryItems = checkedInFlag ? [URLQueryItem(name: "deviceID", value: deviceID), URLQueryItem(name: "visitorEmail", value: visitorEmail),URLQueryItem(name: "isMaunalUpdate", value: "0"),URLQueryItem(name: "closeContactlist", value: "{}")]: [URLQueryItem(name: "deviceID", value: deviceID), URLQueryItem(name: "visitorEmail", value: visitorEmail)]
        
        
        
        urlComps.queryItems = queryItems
        
        print (urlComps.url!)
        
        var request = URLRequest(url: urlComps.url!)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    if let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any]{
                        print(json)
                        if let result = json["result"] as? String {
                            
                            if (result != "success"){
                                DispatchQueue.main.async {
                                    let alert = UIAlertController(title: "Alert", message: "The operation failed. \n Please ask staff for help." , preferredStyle: UIAlertController.Style.alert)
                                    
                                    // add the actions (buttons)
                                    alert.addAction(UIAlertAction(title: "OK", style: UIAlertAction.Style.cancel, handler: nil))
                                    
                                    // show the alert
                                    self.present(alert, animated: true, completion: nil)
                                    
                                    
                                }
                                
                            } else{
                                DispatchQueue.main.async {
                                    self.checkedInFlag.toggle()
                                }
                            }
                            print(result)
                        }
                    }
                
                    
                    
                } catch {
                    print(error)
                    DispatchQueue.main.async {
                        let alert = UIAlertController(title: "Alert", message: "The operation failed. \n Please ask staff for help." , preferredStyle: UIAlertController.Style.alert)
                        
                        // add the actions (buttons)
                        alert.addAction(UIAlertAction(title: "OK", style: UIAlertAction.Style.cancel, handler: nil))
                        
                        // show the alert
                        self.present(alert, animated: true, completion: nil)
                        
                        
                    }
                }
            }
            
        }.resume()
    }
    
    func selfCheck(userEmail: String, userPassword: String){
        
        var urlComps = URLComponents(string: "http://ivmsp.us-east-1.elasticbeanstalk.com/Home/checkUserStatus")!
        
        
        let queryItems = [URLQueryItem(name: "userEmail", value: userEmail), URLQueryItem(name: "userPassword", value: userPassword)]
        urlComps.queryItems = queryItems
        
        print (urlComps.url!)
        
        var request = URLRequest(url: urlComps.url!)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    
                    if let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] {
                        if let contectEvent = json["1"] as? [String: Any] {
                            if (contectEvent.count >= 3){
                                self.alerted = true
                                
                                let Address = contectEvent["Address"] as! String
                                let StartTime = contectEvent["StartTime"] as! String
                                let EndTime = contectEvent["EndTime"] as! String
                                let alertmsg1 = "You may have contact with a COVID-19 carrier at " + Address
                                let alertmsg2 = " from " + StartTime + " to " + EndTime
                                
                                
                                
                                DispatchQueue.main.async {
                                    
                                    self.textView.text = "You may have contact with a COVID-19 carrier \n Plase see a doctor"
                             //       self.checkINOUTbutton.isEnabled = false
                                    
                                    self.backview.backgroundColor = UIColor.red
                                    // create the alert
                                    let alert = UIAlertController(title: "Alert", message:alertmsg1 + alertmsg2, preferredStyle: UIAlertController.Style.alert)
                                    
                                    // add the actions (buttons)
                                    alert.addAction(UIAlertAction(title: "Received", style: UIAlertAction.Style.cancel, handler: nil))
                                    
                                    // show the alert
                                    self.present(alert, animated: true, completion: nil)
                                    
                                }
                            }
                            
                            print(contectEvent)
                            
                            print(contectEvent.count)
                        }
                        
                        // if success
                        // else alert failed
                        
                        //print(json)
                    }
                }catch {
                    print(error)
                }
            }
            
        }.resume()
        
    }
    
    
    
}
