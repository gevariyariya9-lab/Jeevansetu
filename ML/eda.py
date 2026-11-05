
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from pathlib import Path

# Load the cleaned dataset
df = pd.read_csv("dataset/transfusion_clean.csv")

# Create folder for charts
Path("plots").mkdir(exist_ok=True)

sns.set_theme(style="whitegrid")

# 1. Target class distribution
plt.figure(figsize=(7, 5))
sns.countplot(data=df, x="Donated")
plt.title("Blood Donation Class Distribution")
plt.xlabel("Donated in March 2007 (0 = No, 1 = Yes)")
plt.ylabel("Number of Records")
plt.tight_layout()
plt.savefig("plots/donation_distribution.png")
plt.close()

# 2. Distribution of donation recency
plt.figure(figsize=(7, 5))
sns.histplot(data=df, x="Recency", bins=20, kde=True)
plt.title("Distribution of Donation Recency")
plt.xlabel("Recency (months)")
plt.ylabel("Number of Records")
plt.tight_layout()
plt.savefig("plots/recency_distribution.png")
plt.close()

# 3. Donation frequency versus target
plt.figure(figsize=(7, 5))
sns.boxplot(data=df, x="Donated", y="Frequency")
plt.title("Donation Frequency by Target Class")
plt.xlabel("Donated in March 2007 (0 = No, 1 = Yes)")
plt.ylabel("Previous Donation Frequency")
plt.tight_layout()
plt.savefig("plots/frequency_by_donation.png")
plt.close()

print("EDA completed successfully!")
print("Charts saved in the plots folder:")
print("1. plots/donation_distribution.png")
print("2. plots/recency_distribution.png")
print("3. plots/frequency_by_donation.png")
