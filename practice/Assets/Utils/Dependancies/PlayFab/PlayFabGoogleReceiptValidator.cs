using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


public class PlayFabGoogleReceiptValidator
{
    public event Action OnSuccess;
    public event Action OnFailure;

    public void Validate(string currencyCode, uint? price, string receipt, string signature)
    {
        try
        {
            var request = new ValidateGooglePlayPurchaseRequest
            {
                CurrencyCode = currencyCode,
                PurchasePrice = price,
                ReceiptJson = receipt,
                Signature = signature
            };

            PlayFabClientAPI.ValidateGooglePlayPurchase(request, OnSuccessValidation, OnErrorValidation);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            OnFailure?.Invoke();
        }
    }

    private void OnSuccessValidation(ValidateGooglePlayPurchaseResult result) => OnSuccess?.Invoke();

    private void OnErrorValidation(PlayFabError error) => OnFailure?.Invoke();

}

