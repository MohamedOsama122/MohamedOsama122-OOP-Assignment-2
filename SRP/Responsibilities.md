# SRP Responsibility Analysis

This analysis is based on the supplied `csharp-srp-violations` source and its runner.

## WardBoard
Owns bed state, clinical acuity scoring, pager escalation, handoff presentation, and census CSV export. Clinical policy, notifications, formatting, and export contracts change independently.
Final collaborators: `AcuityScorer` changes with clinical scoring; `PagerEscalationLog` changes with alert policy/storage; `HandoffNoteFormatter` changes with handoff presentation; `WardCensusCsvWriter` changes with CSV schema.

## CheckoutBasket
Owns cart lines and total calculation, coupon-text parsing, gift-card copy, and payment authorization formatting. Pricing, marketing rules, customer copy, and gateway protocol are separate reasons to change.
Final collaborators: `CouponDiscountPolicy` changes with promotion rules; `GiftMessageComposer` changes with customer copy; `PaymentAuthorizationFormatter` changes with authorization format.

## SupportTicket
Owns ticket state, keyword priority classification, SLA calculation, public reply wording, and escalation text. Support playbooks, operations SLA policy, and communication templates are distinct concerns.
Final collaborators: `TicketPriorityClassifier`, `SlaDeadlinePolicy`, `PublicReplyComposer`, and `EscalationBlurbFormatter` each own one of those independently changing policies/formats.

## LoanDesk
Owns loan application data, risk scoring, eligibility, compliance documents, decision letters, and CSV analytics. Underwriting policy, compliance policy, communications, and export schema vary separately.
Final collaborators: `LoanRiskScorer`, `LoanEligibilityPolicy`, `RequiredDocumentPolicy`, `LoanDecisionLetterComposer`, and `UnderwriterCsvFormatter` isolate underwriting, compliance, communication, and export changes.

## CourseEnrollmentDesk
Owns seating and waitlist state, registration/promotion rules, welcome markdown, and invoice formatting. Enrollment operations, marketing content, and finance formatting should be cohesive collaborators.
Final collaborators: `EnrollmentRoster` owns enrollment state; `WelcomePacketComposer` owns marketing content; `TuitionInvoiceFormatter` owns invoice layout and VAT formatting.

## KitchenTicket
Owns order data, allergen detection, kitchen ETA estimation, thermal rendering, and lane selection. Regulation, operations heuristics, printer layout, and dispatch policy change for different reasons.
Final collaborators: `AllergenScanner`, `KitchenReadyTimeEstimator`, `ThermalTicketRenderer`, and `ExpoLaneSelector` each change for one policy or output concern.

## SubscriptionBilling
Owns subscription data, proration, invoice sequencing, failed-payment state, dunning wording, and ledger export. Billing calendar rules, numbering, collection communications, and accounting integration are separate responsibilities.
Final collaborators: `SubscriptionProrationCalculator`, `InvoiceNumberGenerator`, `DunningEmailComposer`, and `LedgerJournalFormatter` isolate those independent reasons to change.

## WarehousePickList
Owns pick needs, stock allocation, walking-route ordering, picker wording, and WMS XML. Allocation policy, route strategy, handheld UX, and integration contract vary independently.
Final collaborators: `StockAllocator`, `WarehouseWalkingRoutePlanner`, `PickerScriptComposer`, and `WmsBatchXmlFormatter` own allocation, routing, UX copy, and integration format respectively.

## GradeBook
Owns score storage, average aggregation, letter/honor policy, transcript rendering, and CSV export. Grade policy, registrar document layout, and data-export shape should not change together.
Final collaborators: `ScoreLedger`, `GradeAverageCalculator`, `LetterGradePolicy`, `HonorRollPolicy`, `TranscriptFormatter`, and `GradeBookCsvExporter` isolate records, policies, and formats.

## AppointmentDesk
Owns booking state, opening-hours validation, slot search, ICS serialization, and SMS wording. Clinic policy, scheduling, calendar interoperability, and messaging are independent responsibilities.
Final collaborators: `ClinicBusinessHoursPolicy`, `AppointmentBookingCalendar`, `AppointmentSlotFinder`, `IcsCalendarSerializer`, and `SmsReminderComposer` each own a specific scheduling or communication concern.
