
import pandas as pd

results = pd.DataFrame([
    {
        "Model": "Logistic Regression",
        "Class Weight": "None",
        "Accuracy": 0.7667,
        "ROC-AUC": 0.7878,
        "Positive Precision": 0.57,
        "Positive Recall": 0.11
    },
    {
        "Model": "Logistic Regression",
        "Class Weight": "Balanced",
        "Accuracy": 0.7200,
        "ROC-AUC": 0.7848,
        "Positive Precision": 0.45,
        "Positive Recall": 0.83
    }
])

results.to_csv("model_comparison.csv", index=False)

print("MODEL COMPARISON")
print(results.to_string(index=False))
print("\nResults saved to model_comparison.csv")
