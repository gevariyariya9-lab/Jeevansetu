
from flask import Flask, request, jsonify
import joblib
import pandas as pd
from pathlib import Path

app = Flask(__name__)

# Load the existing trained model
MODEL_PATH = Path(__file__).parent / "models" / "donation_model.joblib"
model = joblib.load(MODEL_PATH)

FEATURES = ["Recency", "Frequency", "Monetary", "Time"]


@app.route("/", methods=["GET"])
def home():
    return jsonify({
        "service": "JeevanSetu ML Prediction API",
        "status": "running"
    })


@app.route("/predict", methods=["POST"])
def predict():
    data = request.get_json(silent=True)

    if not isinstance(data, dict):
        return jsonify({"error": "A JSON object is required."}), 400

    try:
        values = {feature: float(data[feature]) for feature in FEATURES}
    except (KeyError, TypeError, ValueError):
        return jsonify({
            "error": "Provide numeric Recency, Frequency, Monetary, and Time values."
        }), 400

    if any(value < 0 for value in values.values()):
        return jsonify({"error": "Values cannot be negative."}), 400

    if any(not value.is_integer() for value in values.values()):
        return jsonify({"error": "All input values must be whole numbers."}), 400

    if values["Frequency"] < 1 or values["Time"] < 1:
        return jsonify({
            "error": "Frequency and Time must be at least 1."
        }), 400

    if values["Frequency"] > values["Time"]:
        return jsonify({
            "error": "Frequency cannot exceed Time."
        }), 400

    if values["Monetary"] != values["Frequency"] * 250:
        return jsonify({
            "error": "Monetary must equal Frequency multiplied by 250."
        }), 400

    input_data = pd.DataFrame([values], columns=FEATURES)

    prediction = int(model.predict(input_data)[0])
    probability = float(model.predict_proba(input_data)[0][1])

    return jsonify({
        "prediction": prediction,
        "probability": round(probability, 4),
        "meaning": (
            "Historical target class 1"
            if prediction == 1
            else "Historical target class 0"
        ),
        "notice": (
            "This model predicts a historical dataset target, "
            "not current donor availability or medical eligibility."
        )
    })


if __name__ == "__main__":
    app.run(host="127.0.0.1", port=5000, debug=False)
