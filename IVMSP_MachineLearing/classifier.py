import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix
import pyodbc

class MachineLearningModel(object):
    conn = pyodbc.connect(
        'Driver={SQL Server};'
        'Server=ivmsdb.cs17etkshc9t.us-east-1.rds.amazonaws.com,1433;'
        'Database=ivmsdb;'
        'UID=admin;'
        'PWD=ivmsdbadmin;'
    )

    model = LogisticRegression()

    def __init__(self):
        # Prepare training data
        cursor = self.conn.cursor()
        sql = "SELECT Age, HasInfectedBefore, Periods, CloseContact, ClosePeriods, Status from DataForML;"
        data = pd.read_sql(sql,self.conn)

        x = data[['Age','HasInfectedBefore','Periods','CloseContact','ClosePeriods']]
        y = data["Status"]

        x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.4, random_state=0)

        # Initialize ML model
        self.model.fit(x_train, y_train)

    def predict(self, age, hasInfectedBefore, periods, closeContact, closePeriods):
        return self.model.predict([[age, hasInfectedBefore, periods, closeContact, closePeriods]])

def main():
    print(MachineLearningModel().predict(20, 1, 1000, 0, 0))

if __name__ == "__main__":
    main()

