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
        self.training()

    def training(self):
        # Prepare training data
        cursor = self.conn.cursor()
        sql = "select Age, HasInfectedBefore, Periods, CloseContact, ClosePeriods, Status from DataForML;"
        data = pd.read_sql(sql,self.conn)

        x = data[['Age','HasInfectedBefore','Periods','CloseContact','ClosePeriods']]
        y = data["Status"]

        x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.4, random_state=0)

        # Initialize ML model
        self.model = LogisticRegression()
        self.model.fit(x_train, y_train)

    def predict(self):
        # Collect training data
        cursor = self.conn.cursor()
        sql = "select SourceID, TargetID, Age, HasInfectedBefore, Periods, CloseContact, ClosePeriods, Status from DataForML where HasPredicted = 0 order by TargetID asc;"
        data = pd.read_sql(sql,self.conn)

        # If no new data, it will return directly
        if data.empty:
            return

        x = data[['Age','HasInfectedBefore','Periods','CloseContact','ClosePeriods']]
        y = self.model.predict(x);

        overallPrediction = 0;
        isLast = 0;
        for i in data.index: 
            if i + 1 < data['TargetID'].size and int(data['TargetID'][i]) == int(data['TargetID'][i + 1]):
                if isLast == 1:
                    overallPrediction = int(y[i])
                else :
                    overallPrediction += int(y[i])
                isLast = 0
            else:
                overallPrediction += int(y[i])
                isLast = 1

            # Update prediction result to database
            self.updatePrediction(data['SourceID'][i], data['TargetID'][i], data['Age'][i], data['HasInfectedBefore'][i], data['Periods'][i], data['CloseContact'][i], data['ClosePeriods'][i], overallPrediction, isLast) 
        

    def updatePrediction(self, sourceID, targetID, age, hasInfectedBefore, periods, closeContact, closePeriods, predict, isLast):
        cursor = self.conn.cursor()
        sql = "select UserStatus, predict from HealthStatus where ID = " + str(targetID) + ";"
        data = pd.read_sql(sql,self.conn)

        sql1 = ""
        if isLast == 1 and int(data['UserStatus'][0]) == 0:
            if predict > 0: 
                sql1 = "update HealthStatus set Predict = 1 where ID = " + str(targetID) + ";"
            elif int(data['predict'][0]) > 14:
                sql1 = "update HealthStatus set Predict = 0 where ID = " + str(targetID) + ";"
            elif int(data['predict'][0]) > 0:
                sql1 = "update HealthStatus set Predict = " + str(int(data['predict'][0]) + 1) + " where ID = " + str(targetID) + ";"
        
        sql2 = "update DataForML set HasPredicted = 1 where SourceID = " + str(sourceID) + " and TargetID = " + str(targetID) + " and Age = " + str(age) + " and HasInfectedBefore = " + str(hasInfectedBefore) + " and Periods = " + str(periods) + " and CloseContact = " + str(closeContact) + " and ClosePeriods = " + str(closePeriods) + ";"
        cursor.execute(sql1 + sql2)
        self.conn.commit()

    def predictOnce(self, id, age, hasInfectedBefore, periods, closeContact, closePeriods):

        predictData = self.model.predict([[age, hasInfectedBefore, periods, closeContact, closePeriods]])
        return predictData[0]

def main():
    predictionModel = MachineLearningModel()
    predictionModel.predict()
    # print(predictionModel.predictOnce(1, 50, 0, 1000, 1, 500))
    # print(predictionModel.predictOnce(1, 25, 1, 1000, 0, 0))

if __name__ == "__main__":
    main()

