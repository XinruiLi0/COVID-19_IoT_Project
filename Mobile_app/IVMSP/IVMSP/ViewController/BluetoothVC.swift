////
////  BluetoothVC.swift
////  IVMSP
////
////  Created by Yisheng Li on 2021/1/6.
////
//
//import UIKit
////import CoreBluetooth
//
//
//class BluetoothVC: UIViewController,CBCentralManagerDelegate, ObservableObject, UITableViewDelegate, UITableViewDataSource{
//
//
//    @IBOutlet var tableView: UITableView!
//
//    var myState = ""
//    var devices:[String] = []
//    var centralManager: CBCentralManager!
//
//    override func viewDidLoad() {
//        super.viewDidLoad()
//        centralManager = CBCentralManager(delegate: self, queue: nil)
//        tableView.delegate = self
//        tableView.dataSource = self
//    }
//
//
//    func centralManagerDidUpdateState(_ central: CBCentralManager) {
//        switch central.state {
//        case .unknown:
//            self.myState = "central.state is .unknown"
//            print("central.state is .unknown")
//        case .resetting:
//            self.myState = "central.state is .resetting"
//            print("central.state is .resetting")
//        case .unsupported:
//            self.myState = "central.state is .unsupported"
//            print("central.state is .unsupported")
//        case .unauthorized:
//            self.myState = "central.state is .unauthorized"
//            print("central.state is .unauthorized")
//        case .poweredOff:
//            self.myState = "central.state is .poweredOff"
//            print("central.state is .poweredOff")
//        case .poweredOn:
//            self.myState = "central.state is .poweredOn"
//            print("central.state is .poweredOn")
//            centralManager.scanForPeripherals(withServices: nil)
//
//        default:
//            print("error")
//        }
//
//    }
//
//    public func centralManager(_ central: CBCentralManager, didDiscover peripheral: CBPeripheral, advertisementData: [String : Any], rssi RSSI: NSNumber) {
//        // print("Peripheral Name: \(String(describing: peripheral.name))  RSSI: \(String(RSSI.doubleValue))")
//        print("\(peripheral)")
//        self.devices.append("\(peripheral)")
//        self.tableView.reloadData()
//
//    }
//
//
//    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
//        return devices.count
//    }
//
//    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
//        let cell = tableView.dequeueReusableCell(withIdentifier: "cell", for: indexPath) as! selfSizingTableViewCell
//        cell.cellText.text = devices[indexPath.row]
//
//        return cell
//    }
//
//
//}
