import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface ImporterStatus {
    loading?: true;
    importers: Array<DropdownOption>;
    error?: string;
}