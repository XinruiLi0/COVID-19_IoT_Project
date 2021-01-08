//
//  HomeVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2020/12/27.
//

import UIKit
import CoreNFC

class HomeVC: UIViewController, NFCTagReaderSessionDelegate {
    
    @IBOutlet  var textView: UITextView!
    
    var nfcSession: NFCTagReaderSession?
    
    var userEmail = "1181536731@qq.com"
    var deviceID = "None"{
        didSet{
            
            self.textView.text = deviceID
        }
    }
    
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.hideKeyboardWhenTappedAround()
        
    }
    
    @IBAction func onScanNFC(_ sender: Any) {
        self.nfcSession = NFCTagReaderSession(pollingOption: .iso14443, delegate: self)
        self.nfcSession?.alertMessage = "Please Hold Near NFC Tag"
        self.nfcSession?.begin()
    }
    
    @IBAction func onScanQR(_ sender: Any) {
        performSegue(withIdentifier: "showQRscan", sender: sender)
    }
    
    
    
    func tagReaderSessionDidBecomeActive(_ session: NFCTagReaderSession) {
        print("Session Begun")
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didInvalidateWithError error: Error) {
        print(error)
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didDetect tags: [NFCTag]) {
        print("Detected NFC tag")
        
        if tags.count > 1{
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
                session.alertMessage = "Checked In \n\n Thank You"
                session.invalidate()
                DispatchQueue.main.async {
                    
                    self.deviceID = UID
                    
                    self.sendCheckInRecord(deviceID: self.deviceID, vistorEmail: self.userEmail)
                }
            }
        }
    }
    
    
    func sendCheckInRecord(deviceID: String, vistorEmail: String) {
        let parameters = ["deviceID": deviceID, "vistorEmail": vistorEmail]
        
        guard let url = URL(string: "https://jsonplaceholder.typicode.com/posts") else { return }
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        guard let httpBody = try? JSONSerialization.data(withJSONObject: parameters, options: []) else { return }
        request.httpBody = httpBody
        
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    let json = try JSONSerialization.jsonObject(with: data, options: [])
                    print(json)
                } catch {
                    print(error)
                }
            }
            
        }.resume()
    }
    
    
    
}
