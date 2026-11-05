
import pandas as pd
from pathlib import Path
from sklearn.model_selection import train_test_split
from sklearn.pipeline import make_pipeline
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import (
    accuracy_score,
    classification_report,
    confusion_matrix,
    roc_auc_score
)
import joblib

# Load cleaned dataset
df = pd.read_csv("dataset/transfusion_clean.csv")

# Separate input features and prediction target
X = df[["Recency", "Frequency", "Monetary", "Time"]]
y = df["Donated"]

# Split data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(
    X,
    y,
    test_size=0.20,
    random_state=42,
    stratify=y
)

# Build the model pipeline
model = make_pipeline(
    StandardScaler(),
    LogisticRegression
(
    max_iter=1000,
    random_state=42,
    class_weight="balanced"
)
)
# Train the model
model.fit(X_train, y_train)

# Make predictions
y_pred = model.predict(X_test)
y_prob = model.predict_proba(X_test)[:, 1]

# Evaluate the model
print("MODEL EVALUATION")
print("-" * 40)
print(f"Training records: {len(X_train)}")
print(f"Testing records: {len(X_test)}")
print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
print(f"ROC-AUC: {roc_auc_score(y_test, y_prob):.4f}")
print("\nClassification Report:")
print(classification_report(y_test, y_pred, zero_division=0))
print("Confusion Matrix:")
print(confusion_matrix(y_test, y_pred))

# Save the trained model
Path("models").mkdir(exist_ok=True)
joblib.dump(model, "models/donation_model.joblib")

print("\nModel saved to models/donation_model.joblib")
