# SRP Responsibility Analysis

This analysis is based on the supplied `csharp-srp-violations` source and its runner.

## WardBoard
Owns bed state, clinical acuity scoring, pager escalation, handoff presentation, and census CSV export. Clinical policy, notifications, formatting, and export contracts change independently.

## CheckoutBasket
Owns cart lines and total calculation, coupon-text parsing, gift-card copy, and payment authorization formatting. Pricing, marketing rules, customer copy, and gateway protocol are separate reasons to change.

## SupportTicket
Owns ticket state, keyword priority classification, SLA calculation, public reply wording, and escalation text. Support playbooks, operations SLA policy, and communication templates are distinct concerns.

## LoanDesk
Owns loan application data, risk scoring, eligibility, compliance documents, decision letters, and CSV analytics. Underwriting policy, compliance policy, communications, and export schema vary separately.

## CourseEnrollmentDesk
Owns seating and waitlist state, registration/promotion rules, welcome markdown, and invoice formatting. Enrollment operations, marketing content, and finance formatting should be cohesive collaborators.

## KitchenTicket
Owns order data, allergen detection, kitchen ETA estimation, thermal rendering, and lane selection. Regulation, operations heuristics, printer layout, and dispatch policy change for different reasons.

## SubscriptionBilling
Owns subscription data, proration, invoice sequencing, failed-payment state, dunning wording, and ledger export. Billing calendar rules, numbering, collection communications, and accounting integration are separate responsibilities.

## WarehousePickList
Owns pick needs, stock allocation, walking-route ordering, picker wording, and WMS XML. Allocation policy, route strategy, handheld UX, and integration contract vary independently.

## GradeBook
Owns score storage, average aggregation, letter/honor policy, transcript rendering, and CSV export. Grade policy, registrar document layout, and data-export shape should not change together.

## AppointmentDesk
Owns booking state, opening-hours validation, slot search, ICS serialization, and SMS wording. Clinic policy, scheduling, calendar interoperability, and messaging are independent responsibilities.
