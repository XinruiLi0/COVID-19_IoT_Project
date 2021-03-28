//
//  selfSizingTableViewCell.swift
//  IVMSP
//
//  Created by Yisheng Li on 2021/1/12.
//

import UIKit

class selfSizingTableViewCell: UITableViewCell {

    @IBOutlet var cellText: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
