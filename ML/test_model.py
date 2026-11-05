
import joblib
import pandas as pd

# Load the saved trained model
model = joblib.load("models/donation_model.joblib")

# Sample donor information
sample_donor = pd.DataFrame([{
    "Recency": 2,
    "Frequency": 10,
    "Monetary": 2500,
    "Time": 30
}])

# Predict the dataset-defined target
prediction = model.predict(sample_donor)[0]
probability = model.predict_proba(sample_donor)[0][1]

print("SAMPLE DONOR TEST")
print("-----------------")
print("Recency:", sample_donor.iloc[0]["Recency"], "months")
print("Previous donations:", sample_donor.iloc[0]["Frequency"])
print("Total donated:", sample_donor.iloc[0]["Monetary"], "cc")
print("Time:", sample_donor.iloc[0]["Time"], "months")
print("Predicted target:", int(prediction))
print(f"Model probability for target 1: {probability:.2%}")

if prediction == 1:
    print("Prediction: Target class 1")
else:
    print("Prediction: Target class 0")

print("\nTest completed successfully.")
print("Note: This is a historical dataset prediction,")
print("not confirmation of current donor availability.")
