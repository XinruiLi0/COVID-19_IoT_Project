//
//  HomeVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2020/12/27.
//

import UIKit
import CoreNFC

class HomeVC: UIViewController, NFCTagReaderSessionDelegate {
    func tagReaderSessionDidBecomeActive(_ session: NFCTagReaderSession) {
        print("Session Begun")
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didInvalidateWithError error: Error) {
        print("Error when luna")
    }
    
    func tagReaderSession(_ session: NFCTagReaderSession, didDetect tags: [NFCTag]) {
        print("connect  ")
    }
    

    

    
    @IBOutlet var NFCText: UITextView!
    
    @IBOutlet var scan: UIButton!
    
    var nfcSession: NFCTagReaderSession?
    var word = "None"
    
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
    
    }
    @IBAction func scanButton(_ sender: Any) {
        self.nfcSession = NFCTagReaderSession(pollingOption: .iso14443, delegate: self)
        self.nfcSession?.alertMessage = "Hold near NFC"
        self.nfcSession?.begin()
    }
    
    
    
    
//
//    override func didReceiveMemoryWarning() {
//        super.didReceiveMemoryWarning()
//    }
//
//
//
//    func readerSession(_ session: NFCNDEFReaderSession, didInvalidateWithError error: Error) {
//        print("errorrrrr")
//    }
//
//    func readerSession(_ session: NFCNDEFReaderSession, didDetectNDEFs messages: [NFCNDEFMessage]) {
//        var result = ""
//
//        for payload in messages[0].records{
//            result += String.init(data: payload.payload.advanced(by: 3), encoding: .utf8) ?? "Format not supported"
//        }
//
//        DispatchQueue.main.async {
//            self.NFCText.text = result
//        }
    }

