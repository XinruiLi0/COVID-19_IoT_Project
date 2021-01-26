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
    
    
    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        if let location = locations.first{
            locationManager.stopUpdatingLocation()
            render(location)
        }
        
        let casesLocation1 = MKPointAnnotation()
        casesLocation1.title = "1"
        casesLocation1.coordinate = CLLocationCoordinate2D(latitude: 45.381449, longitude: -75.733208)
        
        let casesLocation2 = MKPointAnnotation()
        casesLocation2.title = "2"
        casesLocation2.coordinate = CLLocationCoordinate2D(latitude: 45.397754, longitude: -75.673615)
        
        let casesLocation3 = MKPointAnnotation()
        casesLocation3.title = "3"
        casesLocation3.coordinate = CLLocationCoordinate2D(latitude: 45.364116, longitude: -75.685741)
        
        let casesLocation4 = MKPointAnnotation()
        casesLocation4.title = "5"
        casesLocation4.coordinate = CLLocationCoordinate2D(latitude: 45.335040, longitude: -75.610077)
        
        
        mapView.addAnnotations([casesLocation1, casesLocation2, casesLocation3, casesLocation4])
    }
    
    func render(_ location: CLLocation){
        let coordinate = CLLocationCoordinate2D(latitude: location.coordinate.latitude, longitude: location.coordinate.longitude)
        
        let span = MKCoordinateSpan(latitudeDelta: 0.1, longitudeDelta: 0.1)
        
        let region = MKCoordinateRegion(center: coordinate, span: span)
        
        mapView.setRegion(region, animated: true)
    }
}
