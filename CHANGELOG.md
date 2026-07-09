# Changelog

## [2026.7.9] - 2026-07-09

- Display total collected penalties in the Case File summary section.
- Update the SBEAP search forms to help avoid invalid search terms.

## [2026.6.18] - 2026-06-18

- Boring back end code updates.

## [2026.6.16] - 2026-06-16

- Made additional tables "collapsible" at small screen sizes.

## [2026.6.5] - 2026-06-05

- Update the compliance search forms to help avoid invalid search terms.

## [2026.6.4] - 2026-06-04

- Add search filters to the source test page.
- Implement "collapsible" tables for search results (makes them easier to read on small screens).

## [2026.6.2] - 2026-06-02

- Fix the missing Facility column on the source tests page.
- Restrict access to source test printouts while they're still open in the IAIP.
- Add a button to the source test page to print with confidential info displayed.
- Show a warning on the source test printout if it's still open or is showing confidential information.
- Remove caching from source tests so they always show the most current data from the IAIP.
- Add dark mode to the printouts.
- Fix the display of facility regulatory data which was broken.

## [2026.5.26] - 2026-05-26

- Enable setting the date an enforcement action review was requested.
- Enable withdrawing an enforcement action review request.
- Integrate the SBEAP dashboard and menu items into the main application.
- Fix various layout issues.

## [2026.5.15] - 2026-05-15

- Fix an issue that prevented FCEs from being deleted.
- Add a quick-find menu for facilities and work items.

## [2026.5.13] - 2026-05-13

- Add a city dropdown to the facility map page.

## [2026.5.12] - 2026-05-12

- Prevent issuing an Enforcement Action if a review has been requested but not yet completed.
- Don't show the "Submit review" button on Enforcement Actions if the Case File has been deleted.

## [2026.5.7] - 2026-05-07

- Fix some EPA data exchange bugs.
- Clarify some Consent Order date fields.

## [2026.5.5] - 2026-05-05

- Improve performance and usability on the facility list and maps pages.
- Improve the performance of Case File violation types.

## [2026.4.28] - 2026-04-28

- Only show Day Zero for HPV Case Files.
- Reduce the number of unnecessary email notifications.

## [2026.4.24] - 2026-04-24

- Add the ability to track the initial review date for reports and ACCs.

## [2026.4.21] - 2026-04-21

- Add icons to the map links.

## [2026.4.20] - 2026-04-20

- Add links to map the company address on Facility pages.

## [2026.4.17] - 2026-04-17

- Fix a layout issue where a page TOC conflicted with the navbar dropdowns.
- Improved the Facility Maps layout.

## [2026.4.15] - 2026-04-15

- Add a Table of Contents to some longer pages.

## [2026.4.14.1] - 2026-04-14

- Fix a bug that prevented some Case Files from being staged for the EPA data exchange.
- Add a Georgia state outline to the Facility Map page.

## [2026.4.14] - 2026-04-14

- Add maps to the individual facility details pages.

## [2026.4.13] - 2026-04-13

- Improve the usability of the map page:
    - Better handling of failed location requests.
    - Excluded portable sources from the map.
    - Added a permalink for the current map view.
    - Resized the map to fit screen height.
- Improve the usability of the facility page text filter:
    - Hint: once only a single facility is shown, press the <kbd>Enter</kbd> key to open that facility.

## [2026.4.10] - 2026-04-10

- Fix some minor layout issues.
- Change some default values when adding a new RMP inspection.

## [2026.4.9] - 2026-04-09

- Add a facility map page.
- Enable editing LON resolved dates.
- Combine SBEAP and Air Web into one application, Voltron-style.
- Fix incorrect validation logic for some date fields.
- Improve performance on several pages.

## [2026.3.19] - 2026-03-19

- Fix incorrect Air Programs displayed in Facilities and Case Files.
- Add quick access fields to all search forms.
- Improve the display of enforcement action reviews.

## [2026.3.11] - 2026-03-11

- Automatically set reporting period dates when changing the report type.

## [2026.3.6] - 2026-03-06

- Add compliance/enforcement managers to appropriate staff drop-downs.
- Update some permissions:
    - Allow compliance staff to delete compliance work.
    - Allow all staff to view both closed and open enforcement cases.
    - Update the Roles description document.

## [2026.3.4] - 2026-03-04

- Fix a bug in data exchange updates.
- Improve the formatting of enforcement action responses.
- Fix the max date on the Notification due date field.

## [2026.3.2] - 2026-03-02

- Grant all new users the General Staff role.

## [2026.2.28] - 2026-02-28

🚀 Application launch.

[2026.2.28]: https://github.com/gaepdit/air-web/releases/tag/v2026.2.28
[2026.3.2]: https://github.com/gaepdit/air-web/releases/tag/v2026.3.2
[2026.3.4]: https://github.com/gaepdit/air-web/releases/tag/v2026.3.4
[2026.3.6]: https://github.com/gaepdit/air-web/releases/tag/v2026.3.6
[2026.3.11]: https://github.com/gaepdit/air-web/releases/tag/v2026.3.11
[2026.3.19]: https://github.com/gaepdit/air-web/releases/tag/v2026.3.19
[2026.4.9]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.9
[2026.4.10]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.10
[2026.4.13]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.13
[2026.4.14]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.14
[2026.4.14.1]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.14.1
[2026.4.15]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.15
[2026.4.17]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.17
[2026.4.20]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.20
[2026.4.21]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.21
[2026.4.24]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.24
[2026.4.28]: https://github.com/gaepdit/air-web/releases/tag/v2026.4.28
[2026.5.5]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.5
[2026.5.7]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.7
[2026.5.12]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.12
[2026.5.13]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.13
[2026.5.15]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.15
[2026.5.26]: https://github.com/gaepdit/air-web/releases/tag/v2026.5.26
[2026.6.2]: https://github.com/gaepdit/air-web/releases/tag/v2026.6.2
[2026.6.4]: https://github.com/gaepdit/air-web/releases/tag/v2026.6.4
[2026.6.5]: https://github.com/gaepdit/air-web/releases/tag/v2026.6.5
[2026.6.16]: https://github.com/gaepdit/air-web/releases/tag/v2026.6.16
[2026.6.18]: https://github.com/gaepdit/air-web/releases/tag/v2026.6.18

[2026.7.9]: https://github.com/gaepdit/air-web/releases/tag/v2026.7.9
