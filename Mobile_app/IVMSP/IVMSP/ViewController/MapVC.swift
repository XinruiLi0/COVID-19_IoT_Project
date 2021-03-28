//
//  MapVC.swift
//  IVMSP
//
//  Created by Yisheng Li on 2021/1/2.
//

import UIKit
import MapKit
import CoreLocation

class MapVC: UIViewController, CLLocationManagerDelegate {
    
    @IBOutlet var mapView: MKMapView!
    
    let locationManager = CLLocationManager()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        locationManager.delegate = self
        locationManager.requestWhenInUseAuthorization()
        locationManager.desiredAccuracy = kCLLocationAccuracyBest
        locationManager.distanceFilter = kCLDistanceFilterNone
        locationManager.startUpdatingLocation()
        
        mapView.showsUserLocation = true
        
    }
    
    override func viewDidAppear(_ animated: Bool) {
            super.viewDidAppear(animated)
            updateMap()
        }
    
    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        if let location = locations.first{
            locationManager.stopUpdatingLocation()
            render(location)
        }
        
    }
    
    func render(_ location: CLLocation){
        let coordinate = CLLocationCoordinate2D(latitude: location.coordinate.latitude, longitude: location.coordinate.longitude)
        
        let span = MKCoordinateSpan(latitudeDelta: 0.1, longitudeDelta: 0.1)
        
        let region = MKCoordinateRegion(center: coordinate, span: span)
        
        mapView.setRegion(region, animated: true)
    }
    
    
    func updateMap() {
        
        
        var urlComps = URLComponents(string: "http://ivmsp.us-east-1.elasticbeanstalk.com/Home/casesLocations")!
        
        
        //        let queryItems = [URLQueryItem(name: "userEmail", value: userEmail), URLQueryItem(name: "userPassword", value: userPassword)]
        //        urlComps.queryItems = queryItems
        
        print (urlComps.url!)
        
        var request = URLRequest(url: urlComps.url!)
        request.httpMethod = "POST"
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        let session = URLSession.shared
        session.dataTask(with: request) { (data, response, error) in
            
            if let data = data {
                do {
                    
                    
                    
                    if let json = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] {
                        if let location = json["0"] as? [String: Any] {
                            
                            
                            let name = location["UserName"] as! String
                            
                            let Address = location["Address"] as! String
                            let latitude = location["latitude"] as! String
                            let longitude = location["longitude"] as! String
                            
                            
                            DispatchQueue.main.async {
                                let casesLocation1 = MKPointAnnotation()
                                casesLocation1.title = name + "\n " + Address
                                
                                casesLocation1.coordinate = CLLocationCoordinate2D(latitude: Double(latitude)!, longitude: Double(longitude)!)
                                
                                
                                
                                
                                self.mapView.addAnnotations([casesLocation1])
                                //                                    self.backview.backgroundColor = UIColor.red
                                //                                    // create the alert
                                //                                    let alert = UIAlertController(title: "Alert", message:alertmsg1 + alertmsg2, preferredStyle: UIAlertController.Style.alert)
                                //
                                //                                    // add the actions (buttons)
                                //                                    alert.addAction(UIAlertAction(title: "Received", style: UIAlertAction.Style.cancel, handler: nil))
                                //
                                //                                    // show the alert
                                //                                    self.present(alert, animated: true, completion: nil)
                                
                            }
                            
                            
                            print(location)
                           
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
