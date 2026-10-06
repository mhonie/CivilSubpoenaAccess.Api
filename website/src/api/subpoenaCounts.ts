import axios from "./axios";

export type SubpoenaCountsByStatus = {
  approved: number;
  rejected: number;
  pendingPayment: number;
  pendingApproval: number;
  error?: string;
};

export async function getSubpoenaCountsByStatus(
  approvedStartDate: string,
  approvedEndDate: string,
  filingStartDate: string,
  filingEndDate: string
): Promise<SubpoenaCountsByStatus> {
  const response = await axios.post("/graphql", {
    query: `
      query(
        $approvedStartDate: DateTime!,
        $approvedEndDate: DateTime!,
        $filingStartDate: DateTime!,
        $filingEndDate: DateTime!
      ) {
        subpoenaStatusCounts(
          approvedStartDate: $approvedStartDate,
          approvedEndDate: $approvedEndDate,
          filingStartDate: $filingStartDate,
          filingEndDate: $filingEndDate
        ) {
          approved
          rejected
          pendingPayment
          pendingApproval
          error
        }
      }
    `,
    variables: {
      approvedStartDate,
      approvedEndDate,
      filingStartDate,
      filingEndDate
    }
  });

  return response.data.data.subpoenaStatusCounts;
}

export type SubpoenaCountsByType = {
  attend: number;
  produce: number;
  error?: string;
};

export async function getSubpoenaCountsByType(
  approvedStartDate: string,
  approvedEndDate: string
): Promise<SubpoenaCountsByType> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $approvedStartDate: DateTime!,
        $approvedEndDate: DateTime!
      ) {
        subpoenaTypeCounts(
          approvedStartDate: $approvedStartDate
          approvedEndDate: $approvedEndDate
        ) {
          attend
          produce
          error
        }
      }
    `,
    variables: {
      approvedStartDate,
      approvedEndDate
    }
  });

  return response.data.data.subpoenaTypeCounts;
}

export type SubpoenaCountByUserType = {
  public: number;
  eFilingAttorney: number;
  eFilingProSe: number;
  cityLaw: number;
  citySolicitor: number;
  filingService: number;
  error?: string;
};

export async function getUserTypeCounts(
  approvedStartDate: string,
  approvedEndDate: string
): Promise<SubpoenaCountByUserType> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $approvedStartDate: DateTime!,
        $approvedEndDate: DateTime!
      ) {
        userTypeCounts(
          approvedStartDate: $approvedStartDate
          approvedEndDate: $approvedEndDate
        ) {
          public
          eFilingAttorney
          eFilingProSe
          cityLaw
          citySolicitor
          filingService
          error
        }
      }
    `,
    variables: {
      approvedStartDate,
      approvedEndDate
    }
  });

  return response.data.data.userTypeCounts;
}

export type PaymentsByCreditCardType = {
  americanExpress: number;
  discoverCard: number;
  mastercard: number;
  visaCard: number;
  inFormaPauperis: number;
  citySolicitor: number;
  cityLaw: number;
  walkIn: number;
  error?: string;
};

export async function getCreditCardPaymentTotals(
  paidStartDate: string,
  paidEndDate: string
): Promise<PaymentsByCreditCardType> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $paidStartDate: DateTime!,
        $paidEndDate: DateTime!
      ) {
        approvedPaymentTypeTotals(
          paidStartDate: $paidStartDate,
          paidEndDate: $paidEndDate
        ) {
          americanExpress
          discoverCard
          mastercard
          visaCard
          inFormaPauperis
          citySolicitor
          cityLaw
          walkIn
          error
        }
      }
    `,
    variables: {
      paidStartDate,
      paidEndDate
    }
  });

  return response.data.data.approvedPaymentTypeTotals;
}

export type FeesByCreditCardType = {
  americanExpress: number;
  discoverCard: number;
  mastercard: number;
  visaCard: number;
  inFormaPauperis: number;
  citySolicitor: number;
  cityLaw: number;
  walkIn: number;
  error?: string;
};

export async function getCreditCardPaymentFees(
  paidStartDate: string,
  paidEndDate: string
): Promise<FeesByCreditCardType> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $paidStartDate: DateTime!,
        $paidEndDate: DateTime!
      ) {
        approvedPaymentTypeFees(
          paidStartDate: $paidStartDate,
          paidEndDate: $paidEndDate
        ) {
          americanExpress
          discoverCard
          mastercard
          visaCard
          inFormaPauperis
          citySolicitor
          cityLaw
          walkIn
          error
        }
      }
    `,
    variables: {
      paidStartDate,
      paidEndDate
    }
  });

  return response.data.data.approvedPaymentTypeFees;
}

export type PaymentTypeCounts = {
  americanExpress: number;
  discoverCard: number;
  mastercard: number;
  visaCard: number;
  inFormaPauperis: number;
  citySolicitor: number;
  cityLaw: number;
  walkIn: number;
  error?: string;
};

export async function getApprovedPaymentTypeCounts(
  approvedStartDate: string,
  approvedEndDate: string
): Promise<PaymentTypeCounts> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $approvedStartDate: DateTime!,
        $approvedEndDate: DateTime!
      ) {
        approvedPaymentTypeCounts(
          approvedStartDate: $approvedStartDate
          approvedEndDate: $approvedEndDate
        ) {
          americanExpress
          discoverCard
          mastercard
          visaCard
          inFormaPauperis
          citySolicitor
          cityLaw
          walkIn
          error
        }
      }
    `,
    variables: {
      approvedStartDate,
      approvedEndDate
    }
  });

  return response.data.data.approvedPaymentTypeCounts;
}

export type PaymentTypeCountsByPeriod = {
  startDate: string;
  endDate: string;
  paymentTypeCount: PaymentTypeCounts;
};

export type PaymentTypeCountsByMonth = {
  paymentTypeCountsByPeriod: PaymentTypeCountsByPeriod[];
  error?: string;
};

export async function getMonthlyPaymentTypeCounts(
  filingStartDate: string,
  filingEndDate: string
): Promise<PaymentTypeCountsByMonth> {

  const response = await axios.post("/graphql", {
    query: `
      query(
        $filingStartDate: DateTime!,
        $filingEndDate: DateTime!
      ) {
        monthlyPaymentTypeCounts(
          filingStartDate: $filingStartDate
          filingEndDate: $filingEndDate
        ) {
          paymentTypeCountsByPeriod {
            startDate
            endDate
            paymentTypeCount {
              americanExpress
              discoverCard
              mastercard
              visaCard
              inFormaPauperis
              citySolicitor
              cityLaw
              walkIn
            }
          }
          error
        }
      }
    `,
    variables: {
      filingStartDate,
      filingEndDate
    }
  });

  return response.data.data.monthlyPaymentTypeCounts;
}