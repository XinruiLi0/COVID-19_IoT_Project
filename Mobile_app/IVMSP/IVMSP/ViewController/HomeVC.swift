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
    var deviceID = "None" {
        didSet{
            self.textView.text = deviceID
        }
    }
    
    var checkedInFlag: Bool = false {
        didSet{
            if checkedInFlag == true {
                self.checkINOUTbutton.setTitle("Check Out", for: .normal)
            } else {
                self.checkINOUTbutton.setTitle("Check In", for: .normal)
            }
            
        }
    }
    
    
    @IBOutlet  var textView: UITextView!
    
    @IBOutlet var checkINOUTbutton: UIButton!
    
    var nfcSession: NFCTagReaderSession?
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.hideKeyboardWhenTappedAround()
        
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
        
        
        let queryItems = [URLQueryItem(name: "deviceID", value: deviceID), URLQueryItem(name: "visitorEmail", value: visitorEmail)]
        urlComps.queryItems = queryItems
        
        print (urlComps.url!)
        
        var request = URLRequest(url: urlComps.url!)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    let json = try JSONSerialization.jsonObject(with: data, options: [])
                    
                    // if success
                    DispatchQueue.main.async {
                        self.checkedInFlag.toggle()
                    }
                    // else alert failed
                    
                    print(json)
                } catch {
                    print(error)
                }
            }
            
        }.resume()
    }
    
    
    
}
