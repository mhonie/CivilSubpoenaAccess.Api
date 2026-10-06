import type { 
  PaymentsByCreditCardType, 
  SubpoenaCountsByStatus, 
  SubpoenaCountsByType, 
  SubpoenaCountByUserType, 
  FeesByCreditCardType, 
  PaymentTypeCounts 
} from "../api/subpoenaCounts";

export function totalSubpoenaCountByStatus(
  subpoenaCountsByStatus: SubpoenaCountsByStatus
): number {
  return (
    subpoenaCountsByStatus.approved +
    subpoenaCountsByStatus.rejected +
    subpoenaCountsByStatus.pendingPayment +
    subpoenaCountsByStatus.pendingApproval
  );
}

export function totalSubpoenaCountByType(
  subpoenaCountsByType: SubpoenaCountsByType
): number {
  return (
    subpoenaCountsByType.attend +
    subpoenaCountsByType.produce
  );
}

export function totalUserTypeCount(
  subpoenaCountByUserType: SubpoenaCountByUserType
): number {
  return (
    subpoenaCountByUserType.public +
    subpoenaCountByUserType.eFilingAttorney +
    subpoenaCountByUserType.eFilingProSe +
    subpoenaCountByUserType.cityLaw +
    subpoenaCountByUserType.citySolicitor +
    subpoenaCountByUserType.filingService
  );
}

export function totalCreditCardPayments(
  paymentsByCreditCardType: PaymentsByCreditCardType
): number {
  return (
    paymentsByCreditCardType.americanExpress +
    paymentsByCreditCardType.discoverCard +
    paymentsByCreditCardType.mastercard +
    paymentsByCreditCardType.visaCard +
    paymentsByCreditCardType.inFormaPauperis +
    paymentsByCreditCardType.walkIn +
    paymentsByCreditCardType.cityLaw +
    paymentsByCreditCardType.citySolicitor
  );
}

export function totalCreditCardFees(
  feesByCreditCardType: FeesByCreditCardType
): number {
  return (
    feesByCreditCardType.americanExpress +
    feesByCreditCardType.discoverCard +
    feesByCreditCardType.mastercard +
    feesByCreditCardType.visaCard +
    feesByCreditCardType.inFormaPauperis +
    feesByCreditCardType.walkIn +
    feesByCreditCardType.cityLaw +
    feesByCreditCardType.citySolicitor
  );
}

export function totalCreditCardTransactions(
  approvedPaymentTypeCounts: PaymentTypeCounts
): number {
  return (
    approvedPaymentTypeCounts.inFormaPauperis +
    approvedPaymentTypeCounts.walkIn +
    approvedPaymentTypeCounts.cityLaw +
    approvedPaymentTypeCounts.citySolicitor +    
    approvedPaymentTypeCounts.americanExpress +
    approvedPaymentTypeCounts.discoverCard +
    approvedPaymentTypeCounts.mastercard +
    approvedPaymentTypeCounts.visaCard
  );
}

export function formatCount(value: number): string {
  return value.toLocaleString();
}

export function formatCurrency(value: number): string {
  return value.toLocaleString(
    undefined,
    {
      style: "currency",
      currency: "USD"
    }
  );
}