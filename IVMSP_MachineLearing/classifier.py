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

    def predict(self, id, age, hasInfectedBefore, periods, closeContact, closePeriods):
        predictData = self.model.predict([[age, hasInfectedBefore, periods, closeContact, closePeriods]])
        # cursor = self.conn.cursor()
        # sql = "update HealthStatus set Predict = " + str(predictData[0]) + " where ID = " + str(id)
        # cursor.execute(sql)
        # self.conn.commit()
        return predictData[0]

def main():
    predictionModel = MachineLearningModel()
    print(predictionModel.predict(1, 50, 0, 1000, 1, 500))
    print(predictionModel.predict(1, 25, 1, 1000, 0, 0))

if __name__ == "__main__":
    main()

