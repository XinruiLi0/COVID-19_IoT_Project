import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix
import pyodbc
conn = pyodbc.connect(
    'Driver={SQL Server};'
    'Server=ivmsdb.cs17etkshc9t.us-east-1.rds.amazonaws.com,1433;'
    'Database=ivmsdb;'
    'UID=admin;'
    'PWD=ivmsdbadmin;'
)
cursor = conn.cursor()
sql = "SELECT * from DataForML;"


data = pd.read_sql(sql,conn)
print(data)



from sklearn.linear_model import LogisticRegression
logreg = LogisticRegression()

x = data[['Age','HasInfectedBefore','Periods','CloseContact','ClosePeriods']]
y = data["Status"]

x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.4, random_state=0)

print(x)
print(y)

model = LogisticRegression()
model.fit(x_train, y_train)


y_pred = model.predict(x_test)
print(y_pred)
print('Accuracy of logistic regression classifier on test set: {:.2f}'.format(model.score(x_test, y_test)))
confusion_matrix = confusion_matrix(y_test, y_pred)
print(confusion_matrix)

