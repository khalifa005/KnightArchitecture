---
title: Use Strategy Pattern for Interchangeable Algorithms
impact: MEDIUM
impactDescription: Runtime algorithm selection, extensible
tags: patterns, architecture, strategy
---

## Use Strategy Pattern for Interchangeable Algorithms

Use Strategy when you have 3+ interchangeable algorithms with expected growth (payments, exports). Skip for 2 variants with no growth—use simple if/else.

```typescript
interface PaymentStrategy { process(amount: number): Promise<PaymentResult>; }

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private strategies = new Map<string, PaymentStrategy>();
  register(type: string, strategy: PaymentStrategy) { this.strategies.set(type, strategy); }
  process(type: string, amount: number) { return this.strategies.get(type)!.process(amount); }
}
```
