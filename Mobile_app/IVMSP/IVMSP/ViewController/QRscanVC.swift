//
//  QRscanVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2021/1/7.
//

import UIKit
import swiftScan

class QRscanVC: LBXScanViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        
        var style = LBXScanViewStyle()
        style.anmiationStyle = .NetGrid
        style.animationImage = UIImage(named: "CodeScan.bundle/qrcode_scan_part_net")
        scanStyle = style
        
    }
    
    override func handleCodeResult(arrayResult: [LBXScanResult]) {
        if let result = arrayResult.first {
            let msg = result.strScanned
            print("QR scan result:" + msg!)
            
            
            if let tabBar = presentingViewController as? UITabBarController {
                if let presenter = tabBar.viewControllers![0] as? HomeVC {
                    presenter.deviceID = msg!
                    presenter.sendCheckRecord(deviceID: presenter.deviceID, visitorEmail: presenter.userEmail)
                }
            }
            self.dismiss(animated: true, completion: nil)
        }
    }
    
    
}
