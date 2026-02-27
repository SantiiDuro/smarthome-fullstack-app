import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface HomesStatus {
    loading?: true;
    homes: Array<DropdownOption>;
    error?: string;
}